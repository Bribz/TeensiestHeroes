using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if SERVER
/// <summary>
/// Hot/Cold object for server persistant-data
/// </summary>
public class CharacterServerContainer
{
    public ulong UserID;
    public ulong CharacterID;
    public EntityStats ENTITY_DATA;
    public CharacterData CHARACTER_DATA;

    public CharacterServerContainer(ulong uID, ulong charID, EntityStats statData, CharacterData charData)
    {
        UserID = uID;
        CharacterID = charID;
        ENTITY_DATA = statData;
        CHARACTER_DATA = charData;
    }
	
}

#endif