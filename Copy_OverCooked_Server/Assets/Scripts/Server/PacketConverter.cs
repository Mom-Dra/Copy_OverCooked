using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class PacketConverter 
{
    public static byte[] ConvertDataToBytes(params object[] _object)
    {
        byte[] data = new byte[0];
        foreach (object obj in _object)
        {
            data.Concat(Encoding.UTF8.GetBytes(obj.ToString()));
        }
        return data;
    }
}
