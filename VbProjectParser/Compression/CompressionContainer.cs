using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Compression
{
    public class CompressedContainer
    {
        public readonly Byte SignatureByte;
        protected IList<CompressedChunk> _Chunks = new List<CompressedChunk>();
        public CompressedChunk[] Chunks
        {
            get
            {
                return _Chunks.ToArray();
            }
        }
        protected XlBinaryReader reader;

        /// <summary>
        /// Location of the byte after the last byte in the CompressedContainer
        /// </summary>
        protected int CompressedRecordEnd;

        /// <summary>
        /// Location of the next byte in the compressedcontainer
        /// </summary>
        protected int CompressedCurrent = 0;

        public CompressedContainer(XlBinaryReader CompressedData)
        {
            this.reader = CompressedData;

            this.CompressedRecordEnd = CompressedData.Length;

            // Read signature byte

            this.SignatureByte = CompressedData.ReadByte();
            SanityCheckSignatureByte(this.SignatureByte);

            //while (ArrayIndex < CompressedRecordEnd)
            while(!CompressedData.EndOfData)
            {
                var chunk = new CompressedChunk(CompressedData);
                _Chunks.Add(chunk);
            }

            //throw new NotImplementedException();
        }

        protected void SanityCheckSignatureByte(Byte SignatureByte)
        {
            if (this.SignatureByte != (Byte)0x01)
                throw new FormatException(String.Format("Signature byte expected 0x01, but was 0x{0:X}", this.SignatureByte));

        }
        
        public void Decompress(DecompressedBuffer buffer)
        {
            var state = new DecompressionState();

            if(this.SignatureByte == (Byte)0x01)
            {
                ++state.CompressedCurrent;

                foreach(var chunk in _Chunks)
                {
                    state.CompressedChunkStart = state.CompressedCurrent;
                    chunk.Decompress(buffer, state);
                }
            }
            else
            {
                throw new FormatException("Signature byte was not 0x01");
            }
        }

      
    }

}
