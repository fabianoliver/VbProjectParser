using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Compression
{
    // Buffer to write compressed data to
    public class CompressedBuffer : DecompressedBuffer
    {
        public CompressedBuffer(int InitialSizeAllocation = 10000)
            : base(InitialSizeAllocation)
        {
        }

        public override void SetByte(int index, byte value)
        {
            int C = Data.Count();

            if(index > C)
            {
                // add zero bytes temporarily
                for (int i = C; i < index; i++)
                    SetByte(i, 0x00);

                SetByte(index, value);
            }
            else
            {
                base.SetByte(index, value);
            }
        }

    }
}
