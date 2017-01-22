using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Compression
{
    // Buffer to write decompressed data to
    public class DecompressedBuffer
    {
        protected List<Byte> _Data;
        public IEnumerable<Byte> Data
        {
            get
            {
                return _Data;
            }
        }

        public DecompressedBuffer(int InitialSizeAllocation = 10000)
        {
            this._Data = new List<Byte>(InitialSizeAllocation);
        }

        public DecompressedBuffer(byte[] UncompressedData)
        {
            this._Data = UncompressedData.ToList();
        }

        
        public void Add(DecompressedChunk Chunk)
        {
            this._Data.AddRange(Chunk.Data);
        }

        public virtual void SetByte(int index, Byte value)
        {
            int C = Data.Count();

            if (index < C)
            {
                this._Data[index] = value;
            } else if(index  == C)
            {
                this._Data.Add(value);
            }
            else
            {
                throw new ArgumentOutOfRangeException("index", index, "Index must be <= " + C);
            }
        }

        public Byte GetByteAt(int index)
        {
            return _Data.ElementAt(index);
        }

        public Byte[] GetData()
        {
            return Data.ToArray();
        }
    }
}
