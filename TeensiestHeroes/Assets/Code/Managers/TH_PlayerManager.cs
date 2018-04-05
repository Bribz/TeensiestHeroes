using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TH_PlayerManager : IManager
{
    #region Declaration Station
    internal Player CLIENT_PLAYER { get; private set; }
    internal ulong CLIENT_CHARACTER_ID { get; private set; }
    private List<Player> PlayersInMap;
    #endregion

    internal override bool Initialize()
    {
        PlayersInMap = new List<Player>();
        return true;
    }

    internal void SetClientPlayer(Player player)
    {
        CLIENT_PLAYER = player;
    }

    internal void SetClientCharacterID(ulong CharID)
    {
        CLIENT_CHARACTER_ID = CharID;
    }

    /*
    internal bool IsClient(uint myPlayerId)
    {
        if(CLIENT_PLAYER == null)
        {
            Log.Error("Client Player is null!", 26);

            //TODO: Handle Cheating Prevention
            #if !DEBUG_VERBOSE && !DEBUG_ERROR
                Application.Quit();
            #endif

            return false;
        }

        return CLIENT_PLAYER.networkObject.MyPlayerId == myPlayerId;
    }
    */

    internal bool IsClient(ulong CharID)
    {
        
        if (CLIENT_PLAYER == null)
        {
            Log.Error("Client Player is null!", 54);

            //TODO: Handle Cheating Prevention
            #if !DEBUG_VERBOSE && !DEBUG_ERROR
                Application.Quit();
            #endif

            return false;
        }

        return CLIENT_CHARACTER_ID == CharID;
    }

    public void AddPlayer(Player ply)
    {
        PlayersInMap.Add(ply);
    }

    public void RemovePlayer(ulong ID)
    {
        if(ID == GameManager.instance.PlayerManager.CLIENT_PLAYER.networkObject.NetworkId)
        {
            Log.Error("Trying to destroy self via rpc!");
        }
        
        Player ply = PlayersInMap.FirstOrDefault(p => p.networkObject.NetworkId == ID);
        if (ply == null)
        {
            Log.Error("Attempting to destroy Non-Existing Player");
        }

        PlayersInMap.Remove(ply);
        ply.networkObject.Destroy();
    }

    public void RemovePlayer(Player ply)
    {
        if (ply == GameManager.instance.PlayerManager.CLIENT_PLAYER)
        {
            Log.Error("Trying to destroy self via rpc!");
            return;
        }

        if (ply == null)
        {
            Log.Error("Attempting to destroy Non-Existing Player");
            return;
        }

        PlayersInMap.Remove(ply);
        ply.networkObject.Destroy();
    }
}
