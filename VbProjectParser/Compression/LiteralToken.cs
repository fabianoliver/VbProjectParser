using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Compression
{
    // Size: 1 Byte
    public class LiteralToken : Token
    {
        protected readonly Byte Data;

        public LiteralToken(XlBinaryReader Data)
        {
            this.Data = Data.ReadByte();
        }

        public override int GetSizeInBytes()
        {
            // Size of Data
            return sizeof(Byte);
        }

        public override void Decompress(DecompressedBuffer buffer, DecompressionState state)
        {
            var decompressedChunk = new DecompressedChunk(new byte[] { Data });
            buffer.Add(decompressedChunk);

            ++state.DecompressedCurrent;
            ++state.CompressedCurrent;
        }

        /*
        public override void Decompress(IList<byte> Target, int index)
        {
            Target.Add(Data);
        }*/
    }
}
