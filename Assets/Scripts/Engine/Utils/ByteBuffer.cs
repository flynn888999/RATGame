using UnityEngine;
using System.Collections;
using System;

/********************************
 * Author		：	djl
 * Date			：	2014/7/1
 * Version		：	V 0.1.0	
 * buffer处理
 *******************************/
public class ByteBuffer
{
    public byte[] buf;

    private int readIndex = 0;
    private int writeIndex = 0;
    private int markReadIndex = 0;
    private int markWirteIndex = 0;

    private int capacity;

    public int GetWriteIndex()
    {
        return writeIndex;
    }

    private ByteBuffer(int capacity)
    {
        buf = new byte[capacity];
        this.capacity = capacity;
    }

    public ByteBuffer(byte[] bytes)
    {
        buf = bytes;
        this.capacity = bytes.Length;
    }

    public static ByteBuffer Allocate(int capacity)
    {
        return new ByteBuffer(capacity);
    }

    public static ByteBuffer Allocate(byte[] bytes)
    {
        return new ByteBuffer(bytes);
    }

    public void Write(byte[] bytes)
    {
        int total = bytes.Length + writeIndex;
        int len = buf.Length;
        FixSizeAndReset(len, total);
        for (int i = writeIndex, j = 0; i < total; i++, j++)
        {
            buf[i] = bytes[j];
        }
        writeIndex = total;
    }

    private int FixLength(int length)
    {
        int n = 2;
        int b = 2;
        while (b < length)
        {
            b = 2 << n;
            n++;
        }
        return b;
    }

    private byte[] flip(byte[] bytes)
    {
        //如果是小端，则翻转//
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        return bytes;
    }

    private int FixSizeAndReset(int currLen, int futureLen)
    {
        if (futureLen > currLen)
        {
            byte[] newbuf = new byte[FixLength(currLen) * 2];
            Array.Copy(buf, 0, newbuf, 0, currLen);
            buf = newbuf;
            capacity = newbuf.Length;
        }
        return futureLen;
    }

    public void Write(ByteBuffer buffer)
    {
        if (buffer == null) return;
        if (buffer.ReadableBytes() <= 0) return;
        Write(buffer.ToArray());
    }


    public void WriteChar(char value)
    {
        Write(flip(BitConverter.GetBytes(value)));
    }

    public void WriteShort(short value)
    {
        Write(flip(BitConverter.GetBytes(value)));
    }

    public void WriteUshort(ushort value)
    {
        Write(flip(BitConverter.GetBytes(value)));
    }

    public void WriteInt(int value)
    {
        Write(flip(BitConverter.GetBytes(value)));
    }

    public void WriteUint(uint value)
    {
        Write(flip(BitConverter.GetBytes(value)));
    }

    public void WriteLong(long value)
    {
        Write(flip(BitConverter.GetBytes(value)));
    }

    public void WriteUlong(ulong value)
    {
        Write(flip(BitConverter.GetBytes(value)));
    }

    public void WriteFloat(float value)
    {
        Write(flip(BitConverter.GetBytes(value)));
    }

    public void WriteByte(byte value)
    {
        int afterLen = writeIndex + 1;
        int len = buf.Length;
        FixSizeAndReset(len, afterLen);
        buf[writeIndex] = value;
        writeIndex = afterLen;
    }

    public void WriteDouble(double value)
    {
        Write(flip(BitConverter.GetBytes(value)));
    }


    //string不需要大小端转换
    public void WriteString(string value)
    {
        byte[] tBytes = System.Text.Encoding.Unicode.GetBytes(value);
        Write(tBytes);


        Write(System.Text.Encoding.Unicode.GetBytes("0"));
    }


    public byte ReadByte()
    {
        byte b = buf[readIndex];
        readIndex++;
        return b;
    }

    public int ReadByteToInt()
    {
        return ReadByte();
    }

    public byte[] cloneLeftData()
    {
        byte[] clone = new byte[buf.Length - readIndex];
        Array.Copy(buf, readIndex, clone, 0, buf.Length - readIndex);
        return clone;
    }


    private byte[] Read(int len)
    {
        byte[] bytes = new byte[len];
        Array.Copy(buf, readIndex, bytes, 0, len);
        if (BitConverter.IsLittleEndian)
        {
            Debug.Log("就不转");
            Array.Reverse(bytes);
        }
        readIndex += len;
        return bytes;
    }

    public ushort ReadUshort()
    {
        return BitConverter.ToUInt16(Read(2), 0);
    }

    public short ReadShort()
    {
        return BitConverter.ToInt16(Read(2), 0);
    }

    public uint ReadUint()
    {
        return BitConverter.ToUInt32(Read(4), 0);
    }

    public int ReadInt()
    {
        return BitConverter.ToInt32(Read(4), 0);
    }

    public ulong ReadUlong()
    {
        return BitConverter.ToUInt64(Read(8), 0);
    }

    public long ReadLong()
    {
        return BitConverter.ToInt64(Read(8), 0);
    }

    public float ReadFloat()
    {
        return BitConverter.ToSingle(Read(4), 0);
    }

    public char ReadChar()
    {
        return BitConverter.ToChar(Read(2), 0);
    }


    public double ReadDouble()
    {
        return BitConverter.ToDouble(Read(8), 0);
    }

    public void ReadBytes(byte[] disbytes, int disstart, int len)
    {
        int size = disstart + len;
        for (int i = disstart; i < size; i++)
        {
            disbytes[i] = this.ReadByte();
        }
    }

    public void DiscardReadBytes()
    {
        if (readIndex <= 0) return;
        int len = buf.Length - readIndex;
        byte[] newbuf = new byte[len];
        Array.Copy(buf, readIndex, newbuf, 0, len);
        buf = newbuf;
        readIndex = 0;
        writeIndex -= readIndex;
        markReadIndex -= readIndex;
        if (markReadIndex < 0)
        {
            markReadIndex = readIndex;
        }
        markWirteIndex -= readIndex;
        if (markWirteIndex < 0 || markWirteIndex < readIndex || markWirteIndex < markReadIndex)
        {
            markWirteIndex = writeIndex;
        }
    }

    public void Clear()
    {
        buf = new byte[buf.Length];
        readIndex = 0;
        writeIndex = 0;
        markReadIndex = 0;
        markWirteIndex = 0;
    }

    public void MarkReaderIndex()
    {
        markReadIndex = readIndex;
    }

    public void MarkWriterIndex()
    {
        markWirteIndex = writeIndex;
    }

    public void ResetReaderIndex()
    {
        readIndex = markReadIndex;
    }

    public void ResetWriterIndex()
    {
        writeIndex = markWirteIndex;
    }

    public int ReadableBytes()
    {
        return writeIndex - readIndex;
    }

    public byte[] ToArray()
    {
        byte[] bytes = new byte[writeIndex];
        Array.Copy(buf, 0, bytes, 0, bytes.Length);
        return bytes;
    }

    public int GetCapacity()
    {
        return this.capacity;
    }
}
