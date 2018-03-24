using BeardedManStudios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Data object for Character Data. 
/// WARNING: EDIT THE SERIALIZATION AND DESERIALIZATION SHOULD YOU MAKE CHANGES TO THE DECLARED VARIABLES!
/// </summary>
public class CharacterData
{
    #region Declaration_Station
    public string CharacterName     { get; private set; }
    public ulong CharacterID        { get; private set; }
    public uint MapID               { get; private set; }
    public Vector3 UserPosition     { get; private set; }
    public EquipmentData Equipment  { get; private set; }
    #endregion

    /// <summary>
    /// Default Constructor. Avoid Using.
    /// </summary>
    private CharacterData()
    {
        CharacterName = "";
        CharacterID = 0;
        Equipment = null;
        MapID = 0;
        UserPosition = Vector3.zero;
    }

    public CharacterData(string Name, ulong charID, EquipmentData equipData, Vector3 position, uint mapID = 0)
    {
        CharacterName = Name;
        CharacterID = charID;
        Equipment = equipData;
        MapID = mapID;
        UserPosition = position;
    }
	
    public void ChangeEquipment(EquipmentData equipData)
    {
        Equipment = equipData;
    }

    public void ChangeEquipment(WeaponType type, bool isMainHand)
    {
        if(isMainHand)
        {
            Equipment.MainHand = type;
        }
        else
        {
            Equipment.OffHand = type;
        }
    }

    public void ChangeMap(uint mID)
    {
        MapID = mID;
    }

    public void SetUserPosition(Vector3 position)
    {
        UserPosition = position;
    }

    internal BMSByte Serialize()
    {
        byte charNameSize = (byte)CharacterName.Length;

        BMSByte retVal = new BMSByte();
        retVal.Append(new byte[] { charNameSize });
        retVal.Append(Encoding.UTF8.GetBytes(CharacterName));
        retVal.Append(BitConverter.GetBytes(CharacterID));
        retVal.Append(BitConverter.GetBytes(MapID));
        retVal.Append(BitConverter.GetBytes(UserPosition.x));
        retVal.Append(BitConverter.GetBytes(UserPosition.y));
        retVal.Append(BitConverter.GetBytes(UserPosition.z));
        retVal.Append(Equipment.Serialize());
        
        return retVal;
    }

    internal void Deserialize(byte[] byteData)
    {
        int currentIndex = 0;
        int charNameSize = (int)byteData[0];
            //BitConverter.ToInt32(byteData, 0);
        currentIndex ++;
        byte[] CharNameBlockCopy = new byte[charNameSize];
        Array.Copy(byteData, 1, CharNameBlockCopy, 0, charNameSize);
        
        CharacterName = UTF8Encoding.UTF8.GetString(CharNameBlockCopy);
        currentIndex += charNameSize;

        CharacterID = BitConverter.ToUInt64(byteData, currentIndex);
        currentIndex += sizeof(ulong);

        MapID = BitConverter.ToUInt32(byteData, currentIndex);
        currentIndex += sizeof(ulong);

        float mapPosX = BitConverter.ToSingle(byteData, currentIndex);
        currentIndex += sizeof(float);
        float mapPosY = BitConverter.ToSingle(byteData, currentIndex);
        currentIndex += sizeof(float);
        float mapPosZ = BitConverter.ToSingle(byteData, currentIndex);
        currentIndex += sizeof(float);

        UserPosition = new Vector3(mapPosX, mapPosY, mapPosZ);

        byte[] EquipData = new byte[12]; //SIZEOF(CHARACTERDATA) //TODO: HANDLE THIS LATER
        Array.Copy(byteData, currentIndex, EquipData, 0, 12);

        Equipment = new EquipmentData();
        Equipment.Deserialize(EquipData);

    }
}
