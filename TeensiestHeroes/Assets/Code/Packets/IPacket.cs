using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


internal enum PACKET_TYPE : ushort
{
    LOGIN               =   0xF0,
    CHAR_PICK           =   0xF1,
    CHAR_DATA           =   0xF2,
    DEFAULT             =   0xFF
}

public abstract class IPacket
{
    internal virtual ushort PACKET_ID
    {
        get
        {
            return (ushort)PACKET_TYPE.DEFAULT;
        }
    }

    public IPacket()
    {

    }

    internal PACKET_TYPE GetPacketType(byte[] DATA)
    {
        return (PACKET_TYPE) BitConverter.ToUInt16(DATA, 0);
    }

    internal virtual BMSByte Serialize()
    {
        BMSByte retObj = new BMSByte();
        return retObj;
    }

    /*
    internal static T DeSerialize<T>()
    {
        T retObj = default(T);
        return retObj;
    }
    */
}
