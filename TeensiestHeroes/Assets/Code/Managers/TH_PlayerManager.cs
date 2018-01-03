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

    public void SetClientPlayer(Player player)
    {
        CLIENT_PLAYER = player;
    }

}
