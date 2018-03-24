using BeardedManStudios;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data object for Equipment Data. 
/// WARNING: EDIT THE SERIALIZATION AND DESERIALIZATION SHOULD YOU MAKE CHANGES TO THE DECLARED VARIABLES!
/// </summary>
public class EquipmentData
{
    public WeaponType MainHand  { get; set; }
    public WeaponType OffHand   { get; set; }
    public WeaponType Tool      { get; set; }

    public BMSByte Serialize()
    {
        BMSByte retVal = new BMSByte();

        retVal.Append(BitConverter.GetBytes((uint)MainHand));
        retVal.Append(BitConverter.GetBytes((uint)OffHand));
        retVal.Append(BitConverter.GetBytes((uint)Tool));

        return retVal;
    }

    public void Deserialize(byte[] byteData)
    {
        MainHand = (WeaponType) BitConverter.ToUInt32(byteData, 0);
        OffHand = (WeaponType) BitConverter.ToUInt32(byteData, sizeof(uint));
        Tool = (WeaponType) BitConverter.ToUInt32(byteData, sizeof(uint));
    }
}
