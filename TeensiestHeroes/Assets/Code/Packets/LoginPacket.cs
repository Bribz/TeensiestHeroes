using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPacket : IPacket
{
    #region DATA
    public ulong UserID;
    #endregion

    internal override ushort PACKET_ID
    {
        get
        {
            return (ushort)PACKET_TYPE.LOGIN;
        }
    }

    public LoginPacket()
    {
        UserID = ulong.MinValue;
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
        packet.UserID = BitConverter.ToUInt64(data, sizeof(ushort)); // Offset by PACKET_ID

        return packet;
    }

}
