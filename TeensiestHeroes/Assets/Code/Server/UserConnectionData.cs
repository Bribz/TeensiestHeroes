using BeardedManStudios.Forge.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserConnectionData
{
    public ulong UserID { get; set; }
    public NetWorker UserNetWorker { get; set; }
    public NetworkingPlayer UserNetPlayer { get; set; }

    public UserConnectionData(ulong ID, NetWorker networker, NetworkingPlayer netplayer)
    {
        UserID = ID;
        UserNetWorker = networker;
        UserNetPlayer = netplayer;
    }


}
