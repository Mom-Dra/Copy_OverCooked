using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Packet : IDisposable
{
    public EActionCode actionCode;
    public int targetId;

    private List<byte> buffer;
    private byte[] readableBuffer;

    private int readPos = 0;

    public Packet(EActionCode actionCode, int targetId)
    {
        this.actionCode = actionCode;
        this.targetId = targetId;

        buffer = new List<byte>();

        Write((int)actionCode);
        Write(targetId);
    }

    public Packet(byte[] bytes)
    {
        readableBuffer = bytes;

        Read(out int action);
        Read(out int target);
        Read(out targetId);

        actionCode = (EActionCode)action;
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
        if (!CanRead())
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

    public override string ToString()
    {
        int length = 0;
        if (readableBuffer != null)
        {
            length = readableBuffer.Length;
        }
        else if (buffer != null)
        {
            length = buffer.Count;
        }
        return $"ActionCode: {actionCode}, TargetId: {targetId}, BufferLength: {length}";
    }
}

