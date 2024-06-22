using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Compression
{
    // TODO!
    // page 57
    public class CompressedChunkData
    {
        public Byte[] Data;
        protected readonly CompressedChunkHeader header;

        public readonly Action<DecompressedBuffer, DecompressionState> Decompress;

        //protected TokenSequence _TokenSequence;

        public CompressedChunkData(CompressedChunkHeader header, XlBinaryReader Data)
        {
            this.header = header;

            if(header.CompressedChunkFlag == (Byte)0x00)
            {
                // page 57: CompressedChunkData contains an array of CompressedChunkHeader.CompressedChunkSize
                // elements plus 3 bytes of uncompressed data
                int ArraySize = header.CompressedChunkSize + 3;
                this.Data = new Byte[ArraySize];
                int CopySize = header.CompressedChunkSize; // todo: this is my fix...
                Array.Copy(Data.ReadBytes(CopySize), 0, this.Data, 0, CopySize);

                //Array.Copy(Data, ArrayIndex, this.Data, 0, CopySize);
                //ArrayIndex += header.CompressedChunkSize;

                this.Decompress = (buffer, state) => DecompressRawChunk(buffer,state,this.Data);


            } else if(header.CompressedChunkFlag == (Byte)0x01)
            {
                // TODO: DO something like when compressedcurrent < compressedend -> add a new tokensequence

                // page 57: CompressedChunkData contains an array of TokenSequence elements
                // Subtract 2 from the header.CompressedChunkSize since have already read 
                // two bytes from the CompressedChunk data when the header was read.
                var size = Math.Min(header.CompressedChunkSize - 2, Data.Length - Data.i);
                var tokenSequences = new List<TokenSequence>();

                int processedBytes = 0;
                while(processedBytes < size)
                {
                    int remainingBytes = size - processedBytes;
                    var tokenSequence = new TokenSequence(Data, remainingBytes);
                    tokenSequences.Add(tokenSequence);
                    processedBytes += tokenSequence.GetSizeInBytes();
                }

             
                //var tokenSequenceBytes = tokenSequence.GetDataInRawBytes();
                //this.Data = tokenSequenceBytes;
                //var tokenSequenceSize = tokenSequenceBytes.Count();

                //if(tokenSequenceSize != header.CompressedChunkSize+3)
                //{
                    //throw new InvalidOperationException(String.Format("CompressedChunkData Data-array size expected {0}, but was {1}", header.CompressedChunkSize+3, tokenSequenceSize));
                //}

                this.Decompress = (buffer, state) => DecompressTokenSequence(buffer, state, tokenSequences);
            }
            else
            {
                throw new Exception();
            }

            //this._TokenSequence = new TokenSequence(ref Data);
            //Data = Data.Skip(N);
        }

        public void DecompressRawChunk(DecompressedBuffer buffer, DecompressionState state, Byte[] Data)
        {
            throw new NotImplementedException();

            Byte[] append = new Byte[4096];
            //Array.Copy(state.OriginalData, state.CompressedCurrent, append, 0, header.CompressedChunkSize);
            
            //int nCopy = Math.Min(4096, state.OriginalData.Length-state.CompressedCurrent);
            //Array.Copy(state.OriginalData, state.CompressedCurrent, append, 0, nCopy);
            Array.Copy(this.Data, 0, append, 0, 4096); // todo: maybe copy from original data?
            state.DecompressedCurrent += 4096;
            state.CompressedCurrent += 4096;

            var x = new DecompressedChunk(append);
            buffer.Add(x);
        }

        protected void DecompressTokenSequence(DecompressedBuffer buffer, DecompressionState state, IEnumerable<TokenSequence> tokenSequence)
        {
            foreach (var t in tokenSequence)
            {
                t.Decompress(buffer, state);
            }
        }
/*
        public void Decompress(IList<Byte> ToTarget)
        {
            if (header.CompressionChunkFlag == (Byte)0x01)
            {
                Console.WriteLine("Using TokenSequence Decompression");
                DecompressTokenSequence(ToTarget);
            }
            else
            {
                Console.WriteLine("Using RawChunkg Decompression");
                throw new NotImplementedException();
            }
        }

        protected void DecompressTokenSequence(IList<Byte> ToTarget)
        {
            _TokenSequence = new TokenSequence(this.Data);
            _TokenSequence.Decompress(ToTarget);
        }*/
    }
}
