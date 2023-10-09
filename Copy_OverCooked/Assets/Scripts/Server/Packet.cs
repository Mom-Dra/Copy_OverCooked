using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum EActionCode
{
    Input,
    Event,
    Transform,
    Instantiate,
    Active,
    Animation,
    Sound
}

public enum ETargetType
{
    Player,
    Object,
    UI
}

public class Packet
{
    public int clientId; // 송신자 or 수신자
    public EActionCode actionCode;
    public ETargetType targetType;
    public int targetId;

    public byte[] rawParmas = new byte[0];

    private byte[] data = new byte[0];

    private int index = 0;

    public void WritePosition(Vector3 pos)
    {
        data.Concat(pos);
    }

    public void WriteFloat(float f)
    {
        data[index]
    }
    
}

