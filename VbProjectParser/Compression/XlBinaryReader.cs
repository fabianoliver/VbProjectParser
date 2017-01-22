using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Compression
{
    public class XlBinaryReader
    {
        protected readonly Byte[] Data;
        public int i { get; protected set; }

        public int Length
        {
            get
            {
                return Data.Length;
            }
        }

        public bool EndOfData
        {
            get
            {
                return this.i >= Length;
            }
        }

        public XlBinaryReader(ref Byte[] input)
        {
            this.Data = input;
            i = 0;
        }

        public Byte[] GetUnreadData()
        {
            int N = this.Length - i;
            var result = new Byte[N];
            Array.Copy(this.Data, i, result, 0, N);
            return result;
        }

        public object Read(Type type)
        {
            if (type == typeof(UInt16))
                return ReadUInt16();
            else if (type == typeof(Int16))
                return ReadInt16();
            else if (type == typeof(UInt32))
                return ReadUInt32();
            else if (type == typeof(Int32))
                return ReadInt32();
            else if (type == typeof(bool))
                return ReadBool();
            else if (type == typeof(byte))
                return ReadByte();
            else if (type == typeof(Guid))
                return ReadGuid();
            else
                throw new InvalidCastException();
        }

        public object ReadArray(Type type, int size)
        {
            var result = Array.CreateInstance(type, size);

            for(int i =0; i<size; i++)
            {
                result.SetValue(Read(type), i);
            }

            return result;
        }

        public Int32 ReadInt32()
        {
            var bytes = Read(sizeof(Int32));
            return BitConverter.ToInt32(bytes,0);
        }

        public UInt32 ReadUInt32()
        {
            var bytes = Read(sizeof(UInt32));
            return BitConverter.ToUInt32(bytes, 0);
        }

        public Int16 ReadInt16()
        {
            var bytes = Read(sizeof(Int16));
            return BitConverter.ToInt16(bytes, 0);
        }

        public UInt16 ReadUInt16()
        {
            var bytes = Read(sizeof(UInt16));
            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        /// Peets at the next UInt16, but does not progress the pointer
        /// </summary>
        public UInt16 PeekUInt16()
        {
            var bytes = Read(i, sizeof(UInt16));
            return BitConverter.ToUInt16(bytes, 0);
        }

        public bool ReadBool()
        {
            var bytes = Read(sizeof(bool));
            return BitConverter.ToBoolean(bytes, 0);
        }

        public byte ReadByte()
        {
            var bytes = Read(1);
            return bytes[0];
        }

        public Guid ReadGuid()
        {
            var bytes = ReadBytes(16);
            var guid = new Guid(bytes);
            return guid;
        }

        /// <summary>
        /// Peeks at the next byte, but does not progress the pointer
        /// </summary>
        public byte PeekByte()
        {
            return ReadByteAt(this.i);
        }

        /// <summary>
        /// Reads the byte at the given index
        /// </summary>
        public byte ReadByteAt(UInt32 index)
        {
            if (index > Int32.MaxValue)
                throw new InvalidOperationException();

            var bytes = Read(Convert.ToInt32(index), 1);
            return bytes[0];
        }

        public byte ReadByteAt(Int32 index)
        {
            return ReadByteAt(Convert.ToUInt32(index));
        }

        public byte[] ReadBytes(int length)
        {
            return ReadBytes(Convert.ToUInt32(length));
        }

        public byte[] ReadBytes(UInt32 length)
        {
            var bytes = Read(length);
            return bytes;
        }

        /// <summary>
        /// Reads data of length $length from the current index (this.i), and automatically advances the current index
        /// </summary>
        protected Byte[] Read(UInt32 length)
        {
            if (length > Int32.MaxValue)
                throw new InvalidOperationException();

            StackFrame frame = new StackFrame(2);
            var method = frame.GetMethod();

            Console.WriteLine("{0}.{1} - reading {2} bytes", method.DeclaringType, method.Name, length);

            int _length = Convert.ToInt32(length);

            var result = Read(this.i, _length);
            this.i += _length;
            return result;
        }

        /// <summary>
        /// Reads data of length at index. Does not progress the index counter.
        /// </summary>
        protected Byte[] Read(int index, int length)
        {
            Byte[] bytes = new Byte[length];
            Array.Copy(this.Data, this.i, bytes, 0, length);
            return bytes;
        }

        public void OutputAllAsBinary()
        {
            for(int i =0; i<Length; i++)
            {
                Console.WriteLine("{0:00}: {1} (0x{2:X})", i, Data[i].ToBitString(), Data[i]);
            }
 
        }

    }
}
