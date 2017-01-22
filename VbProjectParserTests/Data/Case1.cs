using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParserTests.Data
{
    /// <summary>
    /// No Compression example, see page 103
    /// </summary>
    public class Case1 : BaseCase
    {
        public Case1()
        {
            this.UncompressedData = new byte[] { 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x2E };

            this.CompressedData = new byte[] {
                // CompressedContainer
                0x01, // 1 signature byte

                    // ChunkHeader
                    0x19, 0xB0,  // 2 header bytes. Size = 25+3 = 28

                        // First TokenSequence (9 Bytes: 1 Flagbyte + 8 elements)
                        0x00, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68,
                        
                        // Second TokenSequence (9 Bytes: 1 Flagbyte + 8 elements)
                        0x00, 0x69, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x00,
                        
                        // Third TokenSequence (8 Bytes: 1 Flagbyte + 7 elements)
                        0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x2E };
        }
    }
}
