using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TH_PlayerManager : IManager
{
    #region Declaration Station
    public Player CLIENT_PLAYER { get; private set; }
    #endregion

    internal override bool Initialize()
    {
        return true;
    }

    internal void SetClientPlayer(Player player)
    {
        CLIENT_PLAYER = player;
    }

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
}
