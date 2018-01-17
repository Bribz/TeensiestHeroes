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
    private bool connected;
    private ushort port;
    //
    internal NetWorker ServerNetWorker { get; private set; }
    private string IP = "73.246.7.34";
    private bool useMainThreadManagerForRPCs = true;
    public bool DontChangeSceneOnConnect = false;
    private string natServerHost = string.Empty;
    private ushort natServerPort = 7735;
    public bool useInlineChat = false;
    private NetworkManager mgr = null;
    [HideInInspector] public GameObject networkManager = null;
    //

    internal bool Initialize()
    {
        connected = true;

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
            Debug.Log("Player " + player.NetworkId + " timed out");
        };
        //LobbyService.Instance.Initialize(server);

        ServerNetWorker = server;

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

        ((UDPServer)NetworkManager.Instance.Networker).Send(NetworkManager.Instance.Networker.FindPlayer(p => p.Networker == ServerNetWorker), bin);
    }

    private void ReadPacket(NetworkingPlayer player, Binary frame)
    {
        if (frame.GroupId < MessageGroupIds.START_OF_GENERIC_IDS + (int) PACKET_TYPE.LOGIN 
            || frame.GroupId > MessageGroupIds.START_OF_GENERIC_IDS + (int) PACKET_TYPE.DEFAULT)
        {
            return;
        }
        
        //Handle Custom packets
        //TODO: Handle Packet Receiving
        #if SERVER
        
        #else
        switch(frame.GroupId)
        {
            case (int)PACKET_TYPE.LOGIN:
                {
                    LoginPacket packet = LoginPacket.DeSerialize(frame.GetData(false, player));
                    USER_ID = packet.UserID;
                    break;
                }
        }
        #endif
    }

#if SERVER
    private void Server_playerAccepted(NetworkingPlayer player, NetWorker sender)
    {
        LoginPacket packet = new LoginPacket();
        //packet.UserID = 0;

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
