using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class Packet : IDisposable
{
    private EActionCode actionCode;
    private int targetId;

    private List<byte> buffer;
    private byte[] bufferForRead;
    private int readPos = 0;

    private List<string> debugLines;

    // Property
    public EActionCode ActionCode { get => actionCode; }
    public int TargetId { get => targetId; }


    public Packet(EActionCode actionCode, int targetId)
    {
        this.actionCode = actionCode;
        this.targetId = targetId;

        buffer = new List<byte>();
        debugLines = new List<string>();

        Write((int)actionCode);
        Write(targetId);
    }

    public Packet(byte[] bytes)
    {
        bufferForRead = bytes;
        Read(out int action);
        Read(out targetId);

        actionCode = (EActionCode)action;
    }

    public int GetLength()
    {
        if(buffer != null)
        {
            return buffer.Count;
        }
        else if (bufferForRead != null)
        {
            return bufferForRead.Length;
        }

        return 0;
    }

    private bool CanRead()
    {
        return readPos < bufferForRead.Length;
    }

    #region Write

    public void Write(int value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
        debugLines.Add(value.ToString());
    }

    public void Write(float value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
        debugLines.Add(value.ToString());
    }

    public void Write(double value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
        debugLines.Add(value.ToString());
    }

    public void Write(bool value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
        debugLines.Add(value.ToString());
    }

    public void Write(char value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
        debugLines.Add(value.ToString());
    }

    public void Write(byte value)
    {
        buffer.Add(value);
        debugLines.Add(value.ToString());
    }

    public void Write(byte[] value)
    {
        buffer.AddRange(BitConverter.GetBytes(value.Length));
        buffer.AddRange(value);
        debugLines.Add($"[{value}, {value.Length}]");
    }

    public void Write(string value)
    {
        buffer.AddRange(BitConverter.GetBytes(value.Length));
        buffer.AddRange(Encoding.UTF8.GetBytes(value));
        debugLines.Add($"[{value}, {value.Length}]");
    }

    public void Write(Vector3 value)
    {
        buffer.AddRange(BitConverter.GetBytes(value.x));
        buffer.AddRange(BitConverter.GetBytes(value.y));
        buffer.AddRange(BitConverter.GetBytes(value.z));
        debugLines.Add($"[x: {value.x}, y: {value.y}, z: {value.z}]");
    }

    public void Write(Vector2 value)
    {
        buffer.AddRange(BitConverter.GetBytes(value.x));
        buffer.AddRange(BitConverter.GetBytes(value.y));
        debugLines.Add($"[x: {value.x}, y: {value.y}]");
    }

    public void Write(Quaternion value)
    {
        buffer.AddRange(BitConverter.GetBytes(value.x));
        buffer.AddRange(BitConverter.GetBytes(value.y));
        buffer.AddRange(BitConverter.GetBytes(value.z));
        buffer.AddRange(BitConverter.GetBytes(value.w));
        debugLines.Add($"[x: {value.x}, y: {value.y}, z: {value.z}, w: {value.w}]");
    }

    #endregion

    #region Read
    public void Read(out int value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'int'!");
        }

        value = BitConverter.ToInt32(bufferForRead, readPos);
        readPos += 4;
    }

    public void Read(out float value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'float'!");
        }

        value = BitConverter.ToSingle(bufferForRead, readPos);
        readPos += 4;
    }

    public void Read(out double value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'double'!");
        }

        value = BitConverter.ToDouble(bufferForRead, readPos);
        readPos += 8;
    }

    public void Read(out bool value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'bool'!");
        }

        value = BitConverter.ToBoolean(bufferForRead, readPos);
        readPos += 1;
    }

    public void Read(out char value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'char'!");
        }

        value = BitConverter.ToChar(bufferForRead, readPos);
        readPos += 1;
    }

    public void Read(out byte value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'byte'!");
        }

        value = bufferForRead[readPos];
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

        Array.Copy(bufferForRead, readPos, value, 0, length);
    }

    public void Read(out string value)
    {
        if (!CanRead())
        {
            throw new Exception("Could not read value of type 'string'!");
        }

        Read(out int length);

        value = Encoding.ASCII.GetString(bufferForRead, readPos, length);
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
        bufferForRead = null;
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
        if(debugLines != null)
        {
            string result = string.Empty;
            result += "Action: " + debugLines[0] + ", ";
            result += "TargetID: " + debugLines[1] + ", ";
            result += "Args = { ";
            for (int i = 3; i < debugLines.Count; ++i)
            {
                result += debugLines[i] + ", ";
            }

            return result;
        }
        else
        {
            return $"ActionCode: {ActionCode}, TargetId: {TargetId}, BufferLength: {GetLength()}";
        }
    }
}

