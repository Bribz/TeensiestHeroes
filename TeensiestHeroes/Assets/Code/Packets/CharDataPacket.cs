using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharDataPacket : IPacket
{
    #region DATA
    public uint netObjID;
    public CharacterData CharData;
    #endregion


    /// <summary>
    /// Default Constructor for CharacterDataPacket. Try not to use this. 
    /// </summary>
    public CharDataPacket()
    {
        netObjID = uint.MinValue;
        CharData = new CharacterData("", 0, new EquipmentData(), Vector3.zero);
    }

    public CharDataPacket(uint netID, CharacterData cData)
    {
        netObjID = netID;
        CharData = cData;
    }

    internal override ushort PACKET_ID
    {
        get
        {
            return (ushort) PACKET_TYPE.CHAR_DATA;
        }
    }

    internal override BMSByte Serialize()
    {
        BMSByte retVal = new BMSByte();

        retVal.Append(BitConverter.GetBytes(PACKET_ID));
        retVal.Append(BitConverter.GetBytes(netObjID));
        retVal.Append(CharData.Serialize().byteArr);

        return retVal;
    }

    //TODO: Size/length of array copy issue. We send the characterID with the packet, but characterID is stored in characterdata.
    //      Maybe this is redundant? Remove Later?
    internal static CharDataPacket DeSerialize(byte[] data)
    {
         CharDataPacket packet = new CharDataPacket();

        packet.netObjID = BitConverter.ToUInt32(data, sizeof(ushort));    //offset of PACKET_ID
        byte[] charDataArray = new byte[128];
        Array.Copy(data, sizeof(ushort) + sizeof(uint), charDataArray, 0, (data.Length - sizeof(ushort)) - sizeof(uint));
        Array.Resize(ref charDataArray, (data.Length - sizeof(ushort)) - sizeof(uint));
        packet.CharData.Deserialize(charDataArray);

        return packet;
    }
}
