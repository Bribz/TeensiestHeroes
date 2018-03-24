using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountStats : MonoBehaviour
{
    public ulong    uID;
    public ulong    CharacterID;
    public uint     NetPlayerID;
    public uint     NetObjID;

    public void SetAccountStats(ulong _uID, ulong _CharacterID, uint _NetPlayerID, uint _NetObjID)
    {
        uID = _uID;
        CharacterID = _CharacterID;
        NetPlayerID = _NetPlayerID;
        NetObjID = _NetObjID;
    }

    internal bool IsClient()
    {
        #if !SERVER
                return GameManager.instance.PlayerManager.IsClient(CharacterID);
        #else
                return false;
        #endif
    }
}
