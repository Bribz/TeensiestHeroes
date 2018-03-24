using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Called when Player selects a character in select screen
/// </summary>
public class CharPickPacket : IPacket
{
    #region DATA
    public ulong CharID;
    #endregion

    public CharPickPacket()
    {
        CharID = ulong.MinValue;
    }

    internal override ushort PACKET_ID
    {
        get
        {
            return (ushort)PACKET_TYPE.CHAR_PICK;
        }
    }

    internal override BMSByte Serialize()
    {
        BMSByte retVal = new BMSByte();

        retVal.Append(BitConverter.GetBytes(PACKET_ID));
        retVal.Append(BitConverter.GetBytes(CharID));

        return retVal;
    }

    internal static CharPickPacket DeSerialize(byte[] data)
    {
        CharPickPacket packet = new CharPickPacket();
        packet.CharID = BitConverter.ToUInt64(data, sizeof(ushort)); //offset of PACKET_ID

        return packet;
    }

}
