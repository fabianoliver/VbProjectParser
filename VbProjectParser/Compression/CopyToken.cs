using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Compression
{
    // Size: 2 Bytes
    public class CopyToken : Token
    {
        protected readonly ushort Token;

        // Only usable when decompressing
        public struct UnpackedInfo
        {
            public ushort Offset;
            public ushort Length;
        }

        public CopyToken(XlBinaryReader Data)
        {
            this.Token = Data.ReadUInt16();
        }

        public CopyToken(ushort FromValue)
        {
            this.Token = FromValue;
        }

        public UInt16 AsUInt16()
        {
            return this.Token;
        }

        public override int GetSizeInBytes()
        {
            // sizeof Token
            return sizeof(ushort);
        }

        public override void Decompress(DecompressedBuffer buffer, DecompressionState state)
        {
            var info = UnpackCopyToken(this.Token, state.DecompressedCurrent, state.DecompressedChunkStart);

            var copySource = state.DecompressedCurrent - info.Offset;

            // Call Byte Copy (section 2.4.1.3.11) with CopySource, DecompressedCurrent, and Length
            ByteCopy(buffer, copySource, state.DecompressedCurrent, info.Length);

            state.DecompressedCurrent += info.Length;
            state.CompressedCurrent += 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OriginalData"></param>
        /// <param name="CopySource">Specifies the location, in the DecompressedBuffer, of the first byte of the source sequence</param>
        /// <param name="DestinationSource">Specifies the location, in the DecompressedBuffer, of the first byte of the destination sequence</param>
        /// <param name="ByteCount">Specifies the number of bytes to copy. MUST be greater than 0.</param>
        protected void ByteCopy(DecompressedBuffer buffer, int CopySource, int DestinationSource, int ByteCount)
        {
            if (ByteCount <= 0)
                throw new ArgumentOutOfRangeException("ByteCount", ByteCount, "ByteCount must be greater than 0");

            var SrcCurrent = CopySource;
            var DstCurrent = DestinationSource;

            for(int i=1; i<=ByteCount; i++)
            {
                var byteToCopy = buffer.GetByteAt(SrcCurrent);
                //var byteToCopy = OriginalData.ReadByteAt(SrcCurrent);
                buffer.SetByte(DstCurrent, byteToCopy);
                ++SrcCurrent;
                ++DstCurrent;
            }

        }

        public static UnpackedInfo UnpackCopyToken(ushort Token, int DecompressedCurrent, int DecompressedChunkStart)
        {
            var help = CopyTokenHelp(DecompressedCurrent, DecompressedChunkStart);

            ushort length = (ushort)((Token & help.LengthMask) + 3);
            ushort temp1 = (ushort)(Token & help.OffsetMask);
            ushort temp2 = (ushort)((ushort)16 - help.BitCount);
            ushort offset = (ushort)((temp1 >> temp2) + 1);

            return new UnpackedInfo() { Length = length, Offset = offset };
        }

        public UnpackedInfo UnpackCopyToken(int DecompressedCurrent, int DecompressedChunkStart)
        {
            return UnpackCopyToken(this.AsUInt16(), DecompressedCurrent, DecompressedChunkStart);
        }


        public class CopyTokenHelpResult
        {
            public ushort LengthMask;
            public ushort OffsetMask;
            public ushort BitCount;
            public ushort MaximumLength;
        }

        // page 69
        public static CopyTokenHelpResult CopyTokenHelp(int DecompressedCurrent, int DecompressedChunkStart)
        {
            var difference = DecompressedCurrent - DecompressedChunkStart;

            var result = new CopyTokenHelpResult();
            result.BitCount = Math.Max((ushort)Math.Ceiling(Math.Log(difference, 2)), (ushort)4);
            result.LengthMask = (ushort)(0xFFFF >> result.BitCount);
            result.OffsetMask = (ushort)(~result.LengthMask); // bitwise not
            result.MaximumLength = (ushort)((0xFFFF >> result.BitCount) + 3);

            return result;
        }
    }
}
