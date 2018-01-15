using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserConnections
{
#if !Server
    private List<TH_UserConnection> ConnectionList;

    private void Awake()
    {

    }

    /// <summary>
    /// Find UserConnection with corresponding player.
    /// </summary>
    /// <param name="player">Player Obj</param>
    /// <returns>User Connection</returns>
    private TH_UserConnection FindUserConnection(Player player)
    {
        return ConnectionList.FirstOrDefault<TH_UserConnection>(p => p.PLAYER_NET_ID == player.networkObject.NetworkId);
    }

#endif
}
