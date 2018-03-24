#if SERVER
using BeardedManStudios.Forge.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class TH_ServerManager : IManager
{
    private UserConnections CONNECTION_BASE;
    private CharacterBase CHARACTER_DATA_BASE;
    public CharacterFactory CharacterFactory;

    internal override bool Initialize()
    {
        CONNECTION_BASE = sFunc.AddGetComponent<UserConnections>(gameObject);
        CHARACTER_DATA_BASE = sFunc.AddGetComponent<CharacterBase>(gameObject);

        CONNECTION_BASE.Initialize();
        CHARACTER_DATA_BASE.Initialize();
        CharacterFactory = new CharacterFactory();

        return true;
    }

    internal void Update()
    {
        CONNECTION_BASE.pUpdate();
        CHARACTER_DATA_BASE.pUpdate();
    }
    
    #region Userconnection Add/Remove

    public void AddUserConnection(ulong uID, NetWorker networker, NetworkingPlayer netplayer)
    {
        CONNECTION_BASE.AddUserConnection(uID, networker, netplayer);
    }

    public void RemoveUserConnection(ulong uID)
    {
        CONNECTION_BASE.RemoveUserConnection(uID);
    }

    public void RemoveUserConnection(NetWorker networker)
    {
        CONNECTION_BASE.RemoveUserConnection(networker);
    }

    public void RemoveUserConnection(NetworkingPlayer netplayer)
    {
        CONNECTION_BASE.RemoveUserConnection(netplayer);
    }

    #endregion

    #region UserConnection_FindUser

    internal UserConnectionData FindUserConnection(ulong userID)
    {
        return CONNECTION_BASE.FindUserConnection(userID);
    }

    internal UserConnectionData FindUserConnection(NetWorker netWorker)
    {
        return CONNECTION_BASE.FindUserConnection(netWorker);
    }

    internal UserConnectionData FindUserConnection(NetworkingPlayer netplayer)
    {
        return CONNECTION_BASE.FindUserConnection(netplayer);
    }

    #endregion

    #region CharacterBase_Add/Remove/Get

    internal int AddCharacter(ulong userID, ulong charID, EntityStats eData, CharacterData charData)
    {
        int retVal;

        if(!CHARACTER_DATA_BASE.Exists(userID))
        {
            retVal = CHARACTER_DATA_BASE.AddCharacter(userID, charID, eData, charData);
        }
        else
        {
            retVal = CHARACTER_DATA_BASE.FindCharacterIndex(userID);
        }

        return retVal;
    }

    internal void RemoveCharacter_UID(ulong userID)
    {
        CHARACTER_DATA_BASE.RemoveCharacter_UID(userID);
    }
    
    internal void RemoveCharacter_CharID(ulong charID)
    {
        CHARACTER_DATA_BASE.RemoveCharacter_CharID(charID);
    }
    
    internal void RemoveCharacter(CharacterServerContainer obj)
    {
        CHARACTER_DATA_BASE.RemoveCharacter(obj);
    }
    
    internal void RemoveCharacter(int index)
    {
        CHARACTER_DATA_BASE.RemoveCharacter(index);
    }

    internal CharacterData GetCharacterData(int index)
    {
        return CHARACTER_DATA_BASE.GetCharacterData(index);
    }

    internal EntityStats GetPlayerStats(int index)
    {
        return CHARACTER_DATA_BASE.GetPlayerStats(index);
    }

    internal ulong GetCharacterID(ulong userID)
    {
        return CHARACTER_DATA_BASE.GetCharacterID(userID);
    }

    #endregion

}
#endif