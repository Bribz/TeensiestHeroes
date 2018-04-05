using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//#if SERVER
public class CharacterFactory
{
    public GameObject m_CharacterBase;
    private List<GameObject> m_CharacterPool;

    public CharacterFactory()
    {
        /*
        if(!m_CharacterBase)
        {
            Log.Error("Base Character Object was null!", 15);
        }
        */

        m_CharacterPool = new List<GameObject>();
    }

#if SERVER
    public GameObject Server_SpawnCharacter(ulong UserID, CharacterData chData)
    {
        if (!NetworkManager.Instance)
        {
            return null;
        }
        
        //GameObject newCharacter = GameObject.Instantiate(m_CharacterBase);

        var pBh = NetworkManager.Instance.InstantiatePlayer(position: Vector3.zero + Vector3.up);
        pBh.networkObject.AssignOwnership(GameManager.instance.ServerManager.FindUserConnection(UserID).UserNetPlayer);

        GameObject newCharacter = pBh.gameObject;

        GameManager.instance.ServerManager.AddCharacter(UserID, chData.CharacterID, pBh.GetComponent<EntityStats>(), chData);
        AccountStats pBh_Acc = pBh.GetComponent<AccountStats>();

        pBh_Acc.uID = UserID;
        pBh_Acc.CharacterID = chData.CharacterID;
        pBh_Acc.NetPlayerID = pBh.networkObject.MyPlayerId;
        pBh_Acc.NetObjID = pBh.networkObject.NetworkId;

        //TEMP: Initialize with default Weapons
        pBh.GetComponent<AttackHandler>().Initialize(pBh.GetComponent<AttackHandler>().Debug_SwordWeapon, pBh.GetComponent<AttackHandler>().Debug_ShieldWeapon);
        pBh.GetComponent<AttackHandler>().Initialize(pBh.GetComponent<AttackHandler>().Debug_Dash);

        m_CharacterPool.Add(newCharacter);
        //TEMP: return character
        return newCharacter;
    }
    #endif
}
//#endif
