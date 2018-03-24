#if SERVER
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : IManager
{
    #region Declaration_Station
    private List<CharacterServerContainer> CHARACTER_BASE;
    #endregion

    internal override bool Initialize()
    {
        CHARACTER_BASE = new List<CharacterServerContainer>();
        return true;
    }

    internal void UpkeepList()
    {
        //Remove Null Characters
        CHARACTER_BASE.RemoveAll(data => data == null);
    }

    internal void pUpdate()
    {
        //TODO: Upkeep
    }

    /// <summary>
    /// Add Character to internal persistant storage of data
    /// </summary>
    /// <returns>ID in characterBase</returns>
    internal int AddCharacter(ulong userID, ulong charID, EntityStats eData, CharacterData charData)
    {
        int ListID = 0;

        CharacterServerContainer newChar = new CharacterServerContainer(userID, charID, eData, charData);
        CHARACTER_BASE.Add(newChar);
        ListID = CHARACTER_BASE.IndexOf(newChar);

        return ListID;
    }

    /// <summary>
    /// Remove character from persistant data.
    /// </summary>
    /// <param name="userID">User ID</param>
    internal void RemoveCharacter_UID(ulong userID)
    {
        int index = CHARACTER_BASE.FindIndex(data => data.UserID == userID);
        CHARACTER_BASE.RemoveAt(index);
    }

    /// <summary>
    /// Remove character from persistant data.
    /// </summary>
    /// <param name="charID">Character ID</param>
    internal void RemoveCharacter_CharID(ulong charID)
    {
        int index = CHARACTER_BASE.FindIndex(data => data.CharacterID == charID);
        CHARACTER_BASE.RemoveAt(index);
    }

    /// <summary>
    /// Remove character data.
    /// </summary>
    /// <param name="obj">Object from List</param>
    internal void RemoveCharacter(CharacterServerContainer obj)
    {
        CHARACTER_BASE.Remove(obj);
    }

    /// <summary>
    /// Check if a data slot for a user already exists. 
    /// </summary>
    /// <param name="userID">User ID</param>
    /// <returns>Does Exist</returns>
    internal bool Exists(ulong userID)
    {
        return CHARACTER_BASE.Exists(p => p.UserID == userID);
    }

    /// <summary>
    /// Remove character data.
    /// </summary>
    /// <param name="index">Index in List</param>
    internal void RemoveCharacter(int index)
    {
        CHARACTER_BASE.RemoveAt(index);
    }

    /// <summary>
    /// Find an index of a character.
    /// </summary>
    /// <param name="userID">User ID</param>
    /// <returns>Character Index</returns>
    internal int FindCharacterIndex(ulong userID)
    {
        return CHARACTER_BASE.FindIndex(p => p.UserID == userID);
    }

    internal CharacterData GetCharacterData(int index)
    {
        return CHARACTER_BASE[index].CHARACTER_DATA;
    }

    internal EntityStats GetPlayerStats(int index)
    {
        return CHARACTER_BASE[index].ENTITY_DATA;
    }

    internal ulong GetCharacterID(ulong userID)
    {
        return CHARACTER_BASE.First(data => data.UserID == userID).CharacterID;
    }

}

#endif