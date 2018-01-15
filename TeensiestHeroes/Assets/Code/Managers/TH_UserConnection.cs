using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using System;
using BeardedManStudios.Forge.Networking.Unity;

public class TH_UserConnection : UserConnectionBehavior
{
    public ulong USER_ID { get; private set; }
    public ulong PLAYER_NET_ID { get; private set;}
    private bool connected;
    
    internal bool Initialize()
    {
        connected = true;
        return true;
    }


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
        /*
        byte[] playerInfo = args.GetNext<byte[]>();
        ulong userID = BitConverter.ToUInt64(playerInfo, 0);
        if (userID == USER_ID)
        {
            Log.Msg("Received <PlayerJoined> RPC with own UserID. Ignoring...");
            return;
        }
        */

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
}
