using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Compression
{
    public class DecompressedChunk
    {
        public Byte[] Data;

        public DecompressedChunk(Byte[] Data)
        {
            this.Data = Data;
        }
    }
}
