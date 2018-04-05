#if SERVER

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BeardedManStudios.Forge.Networking;

public class UserConnections : IManager
{
    internal List<UserConnectionData> ConnectionList;

    internal override bool Initialize()
    {
        ConnectionList = new List<UserConnectionData>();
        return true;
    }

    internal void AddUserConnection(ulong uID, NetWorker networker, NetworkingPlayer netplayer)
    {
        UserConnectionData newData = new UserConnectionData(uID, networker, netplayer);
        ConnectionList.Add(newData);
    }

    internal void RemoveUserConnection(ulong uID)
    {
        ConnectionList.Remove(ConnectionList.First(p => p.UserID == uID));
    }

    internal void RemoveUserConnection(NetWorker networker)
    {
        ConnectionList.Remove(ConnectionList.First(p => p.UserNetWorker == networker));
    }

    internal void RemoveUserConnection(NetworkingPlayer netplayer)
    {
        ConnectionList.Remove(ConnectionList.First(p => p.UserNetPlayer == netplayer));
    }

    /// <summary>
    /// Find UserConnection with corresponding player.
    /// </summary>
    /// <param name="player">Player Obj</param>
    /// <returns>User Connection</returns>
    internal UserConnectionData FindUserConnection(ulong userID)
    {
        return ConnectionList.FirstOrDefault<UserConnectionData>(p => p.UserID == userID);
    }

    internal UserConnectionData FindUserConnection(NetWorker netWorker)
    {
        return ConnectionList.FirstOrDefault<UserConnectionData>(p => p.UserNetWorker == netWorker);
    }

    internal UserConnectionData FindUserConnection(NetworkingPlayer netplayer)
    {
        return ConnectionList.FirstOrDefault<UserConnectionData>(p => p.UserNetPlayer == netplayer);
    }

    internal void pUpdate()
    {
        //TODO: Upkeep
    }
}

#endif
