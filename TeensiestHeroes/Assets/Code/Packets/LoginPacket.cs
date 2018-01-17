using BeardedManStudios;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPacket : IPacket
{
    #region DATA
    public ulong UserID;
    #endregion
    
    public LoginPacket()
    {
        UserID = ulong.MinValue;
    }

    internal override ushort PACKET_ID
    {
        get
        {
            return 0xF0;
        }
    }

    internal override BMSByte Serialize()
    {
        BMSByte retVal = new BMSByte();

        retVal.Append(BitConverter.GetBytes(PACKET_ID));
        retVal.Append(BitConverter.GetBytes(UserID));
        
        return retVal;
    }

    internal static LoginPacket DeSerialize(byte[] data)
    {
        LoginPacket packet = new LoginPacket();
        packet.UserID = BitConverter.ToUInt64(data, sizeof(ushort));

        return packet;
    }

}
