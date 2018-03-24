using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using System;
using UnityEngine.SceneManagement;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios;
using BeardedManStudios.Forge.Networking.Frame;

public class TH_UserConnection : MonoBehaviour
{
    public ulong USER_ID { get; private set; }
    public ulong PLAYER_NET_ID { get; private set;}
    private bool connected = false;
    private const ushort port = 7735;
    //
    internal NetWorker ServerNetWorker { get; private set; }
    private string IP = "127.0.0.1"; //73.246.7.34";
    private bool useMainThreadManagerForRPCs = true;
    public bool DontChangeSceneOnConnect = false;
    private string natServerHost = string.Empty;
    private ushort natServerPort = 7735;
    public bool useInlineChat = false;
    private NetworkManager mgr = null;
    public GameObject networkManager = null;

    private static ulong currentPlayerIDCount = 1;
    //

    internal bool Initialize()
    {
        connected = true;
        //port = 7735;

        NetWorker.PingForFirewall(port);

        if (useMainThreadManagerForRPCs)
            Rpc.MainThreadRunner = MainThreadManager.Instance;

        return true;
    }

#if !SERVER
    internal void Connect()
    {
        NetWorker client;

        //if (useTCP)
        //{
        //    client = new TCPClient();
        //    ((TCPClient)client).Connect(IP, port);
        //}
        //else
        //{
        client = new UDPClient();
        if (natServerHost.Trim().Length == 0)
            ((UDPClient)client).Connect(IP, port);
        else
            ((UDPClient)client).Connect(IP, port, natServerHost, natServerPort);
        //}
            
        client.binaryMessageReceived += ReadPacket;    

        client.serverAccepted += Client_serverAccepted;
        
        Connected(client);
    }

    private void Client_serverAccepted(NetWorker sender)
    {
        ServerNetWorker = sender;
    }
#else

    public void Host()
    {
        NetWorker server;

        //if (useTCP)
        //{
        //    server = new TCPServer(64);
        //    ((TCPServer)server).Connect();
        //}
        //else
        //{
        server = new UDPServer(64);

        if (natServerHost.Trim().Length == 0)
            ((UDPServer)server).Connect(IP, port);
        else
            ((UDPServer)server).Connect(natHost: natServerHost, natPort: natServerPort);
        //}

        server.playerTimeout += (player, sender) =>
        {
            Log.Msg("Player " + player.NetworkId + " timed out");
            
            //TEMP: Handle removal of clients on timeout.
            //TODO: Handle this with a delay of data unloading.
            GameManager.instance.ServerManager.RemoveUserConnection(sender);

        };
        //LobbyService.Instance.Initialize(server);

        ServerNetWorker = server;

        server.binaryMessageReceived += ReadPacket;

        server.playerAccepted += Server_playerAccepted;

        Connected(server);
    }
#endif

    public void Connected(NetWorker networker)
    {
        if (!networker.IsBound)
        {
            Debug.LogError("NetWorker failed to bind");
            return;
        }

        if (mgr == null && networkManager == null)
        {
            Debug.LogWarning("A network manager was not provided, generating a new one instead");
            networkManager = new GameObject("Network Manager");
            mgr = networkManager.AddComponent<NetworkManager>();
        }
        else if (mgr == null)
            mgr = Instantiate(networkManager).GetComponent<NetworkManager>();

        mgr.Initialize(networker);

        if (useInlineChat && networker.IsServer)
            SceneManager.sceneLoaded += CreateInlineChat;

        if (networker is IServer)
        {
            if (!DontChangeSceneOnConnect)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else
                NetworkObject.Flush(networker); //Called because we are already in the correct scene!
        }
    }

    private void CreateInlineChat(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= CreateInlineChat;
        var chat = NetworkManager.Instance.InstantiateChatManager();
        DontDestroyOnLoad(chat.gameObject);
    }

    private void OnApplicationQuit()
    {
        //if (getLocalNetworkConnections)
        //    NetWorker.EndSession();
    }

    internal void SendPacket(IPacket packet)
    {
        ushort packetID = packet.PACKET_ID;
        if(packetID == (ushort) PACKET_TYPE.DEFAULT)
        {
            Log.Error("Attempted to send default packet!", 146);
        }

        BMSByte dataPackage = packet.Serialize();
        int groupID = MessageGroupIds.START_OF_GENERIC_IDS + packetID;
        //ulong timestep = NetworkManager.Instance.Time.Timestep;
        Binary bin = new Binary(NetworkManager.Instance.Networker.Time.Timestep, false, dataPackage, Receivers.Server, groupID, false);
        var serverPlayer = NetworkManager.Instance.Networker.FindPlayer(p => p.Networker == ServerNetWorker);
        ((UDPClient)NetworkManager.Instance.Networker).Send(bin);
    }

    private void ReadPacket(NetworkingPlayer player, Binary frame, NetWorker sender)
    {
        //Log.Msg("Received Packet", 177);
        if (frame.GroupId < MessageGroupIds.START_OF_GENERIC_IDS + (int) PACKET_TYPE.LOGIN 
            || frame.GroupId > MessageGroupIds.START_OF_GENERIC_IDS + (int) PACKET_TYPE.DEFAULT)
        {
            return;
        }

        byte[] packetData = frame.GetData(false, player);
        ushort packetID = BitConverter.ToUInt16(packetData, 0);

        //Handle Custom packets
#if SERVER
        switch(packetID)
        {
            //TEMP: Handles a character selection from the menu. Currently just spits back a default character.
            //TODO: Connect to database, parse for verbose character data, send back to client.
            case (ushort)PACKET_TYPE.CHAR_PICK:
                {
                    MainThreadManager.Run(() =>
                    {
                        Log.Msg("Received Character Selection Packet", 195);
                        CharPickPacket packet = CharPickPacket.DeSerialize(frame.GetData(false, player));

                        ulong uID = GameManager.instance.ServerManager.FindUserConnection(sender).UserID;

                        CharDataPacket cdPacket = new CharDataPacket(0, new CharacterData("Test", packet.CharID, new EquipmentData(), Vector3.zero));

                        //TODO: Get information for player. 
                        //TEMP: Get CharData from cdpacket. assign character list index to UserConnection as foreign key. Assign entitystats after character object is made. 
                        var newPlayerObj = GameManager.instance.ServerManager.CharacterFactory.Server_SpawnCharacter(uID, cdPacket.CharData);
                        var accData = newPlayerObj.GetComponent<AccountStats>();
                        cdPacket.netObjID = accData.NetObjID;

                        //Redundant Call. Called by CharacterFactory.
                        //int charDataID = GameManager.instance.ServerManager.AddCharacter(uID, 0, newPlayerObj.GetComponent<EntityStats>(), cdPacket.CharData); //HACK: TODO: Fix this line. It doesnt work.

                        SendPacket_Server(cdPacket, player);
                    });
                    break;
                }
        }
#else
        switch(packetID)
        {
            case (ushort)PACKET_TYPE.LOGIN:
                {
                    if(sender != GameManager.instance.UserConnection.ServerNetWorker)
                    {
                        Log.Error("Received Login Packet from a networker besides the server!", 213);
                        break;
                    }
                    MainThreadManager.Run(() => 
                    { 
                        Log.Msg("Received Login Packet", 227);
                        LoginPacket packet = LoginPacket.DeSerialize(frame.GetData(false, player));
                        USER_ID = packet.UserID;
                    
                        //TEMP: Replace with selected Character ID later.
                        GameManager.instance.PlayerManager.SetClientCharacterID(USER_ID);
                        
                        //HACK: Sends character select packet to Server. Assumes that charID is default.
                        CharPickPacket charPacket = new CharPickPacket();
                        charPacket.CharID = packet.UserID;
                        
                        SendPacket(charPacket);
                    });
                    break;
                }
            case (ushort)PACKET_TYPE.CHAR_DATA:
                {
                    MainThreadManager.Run(() =>
                    {
                        Log.Msg("Received Char Data Packet", 246);
                        CharDataPacket packet = CharDataPacket.DeSerialize(frame.GetData(false, player));
                        CharacterData CharacterData = packet.CharData;

                        NetworkBehavior netObj = (NetworkBehavior)NetworkManager.Instance.Networker.NetworkObjects[packet.netObjID].AttachedBehavior;

                        AccountStats accStats = netObj.transform.GetComponent<AccountStats>();
                        accStats.uID = CharacterData.CharacterID;
                        accStats.CharacterID = CharacterData.CharacterID;
                        accStats.NetObjID = packet.netObjID;
                        accStats.NetPlayerID = NetworkManager.Instance.Networker.NetworkObjects[packet.netObjID].MyPlayerId;
                        

                        if (CharacterData.CharacterID == GameManager.instance.PlayerManager.CLIENT_CHARACTER_ID)
                        {
                            //TODO: Set new character to self.
                            GameManager.instance.PlayerManager.SetClientPlayer((Player)netObj);
                        }
                    });
                    break;
                }
        }
#endif
    }

#if SERVER
    private void Server_playerAccepted(NetworkingPlayer player, NetWorker sender)
    {
        LoginPacket packet = new LoginPacket();
        packet.UserID = currentPlayerIDCount;
        
        //TEMP: Create new UserConnection in persistant data. 
        //TODO: Handle this with a call to database to parse userID. Automate adding to persistant data.
        GameManager.instance.ServerManager.AddUserConnection(currentPlayerIDCount, player.Networker, player);

        currentPlayerIDCount += 1;

        SendPacket_Server(packet, player);
    }

    private void SendPacket_Server(IPacket packet, NetworkingPlayer player)
    {
        ushort packetID = packet.PACKET_ID;
        if (packetID == (ushort)PACKET_TYPE.DEFAULT)
        {
            Log.Error("Attempted to send default packet!", 146);
        }

        BMSByte dataPackage = packet.Serialize();
        int groupID = MessageGroupIds.START_OF_GENERIC_IDS + packetID;
        //ulong timestep = NetworkManager.Instance.Time.Timestep;
        Binary bin = new Binary(NetworkManager.Instance.Networker.Time.Timestep, false, dataPackage, Receivers.Target, groupID, false);

        ((UDPServer)NetworkManager.Instance.Networker).Send(player, bin);
    }
#endif

    /*

    /// <summary>
    /// Called when player Disconnects from Server
    /// </summary>
    /// <param name="args">PlayerID</param>
    public override void Disconnect(RpcArgs args)
    {
#if !SERVER
        GameManager.instance.PlayerManager.RemovePlayer(args.GetNext<ulong>());
#else
        networkObject.SendRpc(RPC_DISCONNECT, Receivers.OthersBuffered, args.GetNext<ulong>());
#endif
    }

    /// <summary>
    /// Called when a player joins a map. Only called on clients.
    /// </summary>
    /// <param name="args">(Byte[]) Player Data</param>
    public override void PlayerJoined(RpcArgs args)
    {
        //byte[] playerInfo = args.GetNext<byte[]>();
        //ulong userID = BitConverter.ToUInt64(playerInfo, 0);
        //if (userID == USER_ID)
        //{
        //    Log.Msg("Received <PlayerJoined> RPC with own UserID. Ignoring...");
        //    return;
        //}

        //TODO: Register Player to TH_PlayerManager
        
    }

    
    /// <summary>
    /// Called when a player leaves the map. Only called on clients.
    /// </summary>
    /// <param name="args">(ulong) UserID</param>
    public override void PlayerLeft(RpcArgs args)
    {
        ulong userID = args.GetNext<ulong>();
        if (userID == USER_ID)
        {
            Log.Msg("Received <PlayerLeft> RPC with own UserID. Ignoring...");
            return;
        }
        GameManager.instance.PlayerManager.RemovePlayer(userID);
    }
    

    private void Update ()
    {
		
	}

    private void OnApplicationQuit()
    {

#if !SERVER
        if (connected)
        {
            Application.CancelQuit();
            StartCoroutine(DisconnectSelf());
        }
#else

#endif

    }

    private IEnumerator DisconnectSelf()
    {
        networkObject.SendRpc(RPC_DISCONNECT, Receivers.Server, networkObject.NetworkId);
        NetworkManager.Instance.Disconnect();
        yield return new WaitForFixedUpdate();
        connected = false;
        Application.Quit();
    }

#region Hidden Members
    private void Start() { }
    private void Awake() { }
#endregion

        */
}
