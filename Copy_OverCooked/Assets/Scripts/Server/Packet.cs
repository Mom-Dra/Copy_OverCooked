using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packet
{
    public ushort size;
    public ushort packetId;
}

public class PlayerInfoReq : Packet
{
    public long playerId;
}

public class PlayerInfoOk : Packet
{
    public int hp;
    public int attack;
}

public enum PacketID
{
    PlayerInfoReq = 1,
    PlayerInfoOk = 2,
}
