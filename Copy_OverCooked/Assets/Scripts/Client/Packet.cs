using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting.Dependencies.NCalc;
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

public enum EInputType
{
    Move,
    Alt,
    Space,
    Ctrl
}

public enum ETargetType
{
    Player,
    Object,
    UI
}

public class Packet : IDisposable
{
    public int clientId; // �۽��� or ������
    public EActionCode actionCode;
    public ETargetType targetType;
    public int targetId;

    private List<byte> buffer;
    private byte[] readableBuffer;

    private int readPos = 0;

    public Packet(int clientId, EActionCode actionCode, ETargetType targetType, int targetId)
    {
        this.clientId = clientId;
        this.actionCode = actionCode;
        this.targetType = targetType;
        this.targetId = targetId;

        buffer = new List<byte>();

        Write(clientId);
        Write((int)actionCode);
        Write((int)targetType);
        Write(targetId);
    }

    public Packet(byte[] bytes)
    {
        readableBuffer = bytes;

        Read(out clientId);
        Read(out int action);
        Read(out int target);
        Read(out targetId);

        actionCode = (EActionCode)action;
        targetType = (ETargetType)target;
    }

    private bool CanRead()
    {
        return readPos < readableBuffer.Length;
    }

    #region Write

    public void Write(int value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(float value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(double value) 
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(bool value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(char value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(byte value)
    {
        buffer.Add(value);
    }

    public void Write(byte[] value)
    {
        Write(value.Length);
        buffer.AddRange(value);
    }

    public void Write(string value)
    {
        Write(value.Length);
        buffer.AddRange(Encoding.UTF8.GetBytes(value));
    }

    public void Write(Vector3 value)
    {
        Write(value.x);
        Write(value.y);
        Write(value.z);
    }

    public void Write(Vector2 value)
    {
        Write(value.x);
        Write(value.y);
    }

    public void Write(Quaternion value)
    {
        Write(value.x);
        Write(value.y);
        Write(value.z);
        Write(value.w);
    }

    #endregion

    #region Read
    public void Read(out int value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'int'!");
        }

        value = BitConverter.ToInt32(readableBuffer, readPos);
        readPos += 4;
    }

    public void Read(out float value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'float'!");
        }

        value = BitConverter.ToSingle(readableBuffer, readPos);
        readPos += 4;
    }

    public void Read(out double value)
    {
        if(!CanRead())
        {
            throw new Exception("Could not read value of type 'double'!");
        }

        value = BitConverter.ToDouble(readableBuffer, readPos);
        readPos += 8;
    }

    public void Read(out bool value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'bool'!");
        }

        value = BitConverter.ToBoolean(readableBuffer, readPos);
        readPos += 1;
    }

    public void Read(out char value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'char'!");
        }

        value = BitConverter.ToChar(readableBuffer, readPos);
        readPos += 1;
    }

    public void Read(out byte value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'byte'!");
        }

        value = readableBuffer[readPos];
        readPos += 1;
    }

    public void Read(out byte[] value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'byte[]'!");
        }

        Read(out int length);

        value = new byte[length];

        Array.Copy(readableBuffer, readPos, value, 0, length);
    }

    public void Read(out string value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'string'!");
        }

        Read(out int length);

        value = Encoding.ASCII.GetString(readableBuffer, readPos, length);
        readPos += length;
    }

    public void Read(out Vector2 value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'Vector2'!");
        }

        value = new Vector2();

        Read(out value.x);
        Read(out value.y);
    }

    public void Read(out Vector3 value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'Vector3'!");
        }

        value = new Vector3();

        Read(out value.x);
        Read(out value.y);
        Read(out value.z);
    }

    public void Read(out Quaternion value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'Quaternion'!");
        }

        value = new Quaternion();

        Read(out value.x);
        Read(out value.y);
        Read(out value.z);
        Read(out value.w);
    }

    #endregion


    //public byte[] ToByteArr()
    //{
    //    //Vector2 a = new Vector2();
    //}

    public void Dispose()
    {
        readableBuffer = null;
        buffer = null;
        readPos = 0;

        GC.SuppressFinalize(this);
    }

    public byte[] ToByteArray()
    {
        return buffer.ToArray();
    }
}
