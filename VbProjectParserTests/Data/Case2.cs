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
    public class Case2 : BaseCase
    {
        public Case2()
        {
            // ASCI: #aaabcdefaaaaghijaaaaaklaaamnopqaaaaaaaaaaaarstuvwxyzaaa
            this.UncompressedData = new byte[] { 0x23, 0x61, 0x61, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x61, 0x61, 0x61, 0x61, 0x67, 0x68, 0x69, 0x6a, 0x61, 0x61, 0x61, 0x61, 0x61, 0x6B, 0x6C, 0x61, 0x61, 0x61, 0x6D, 0x6E, 0x6F, 0x70, 0x71, 0x61, 0x61, 0x61, 0x61, 0x61, 0x61, 0x61, 0x61, 0x61, 0x61, 0x61, 0x61, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7A, 0x61, 0x61, 0x61 };

            // CAREFUL!
            // When Compressing this.UncompressedData with the Compression algorithm, it will result in a different byte array
            // than the below (this.CompressedData).
            // however, when its result is de-compressed again, it will return the original UncompressedData.
            // I suspect this is because there are potentially many ways to generate correct Compressed Data structures -
            // e.g. when a CopyToken could copy its values from potentially different previous locations in the DecompressedBuffer that all return the same bytes
            this.CompressedData = new byte[] {
                // CompressedContainer
                // Signature byte = 0x01
                0x01,
                
                // ChunkHeader
                // 0xB02F = 1011000000101111
                // CompressedChunkSize = 47+3 = 50
                // CompressedChunkSignature = 0b011
                // CompressedChunkFlag = 1
                0x2F, 0xB0,
                
                // Tokensequence, size in bytes = 9 = 1 + 8*1
                // Writes #aaabcde
                // Data = 0x61, 0x61, 0x61, 0x62, 0x,63, 0x64, 0x65
                0x00, //FlagByte = 0000 0000 -> All literal tokens
                    0x23,       // LiteralToken, ASCII = #
                    0x61,       // LiteralToken, ASCII = a
                    0x61,       // LiteralToken, ASCII = a
                    0x61,       // LiteralToken, ASCII = a
                    0x62,       // LiteralToken, ASCII = b
                    0x63,       // LiteralToken, ASCII = c
                    0x64,       // LiteralToken, ASCII = d
                    0x65,       // LiteralToken, ASCII = e
                
                // TokenSequence, size in bytes = 11 = 1*sizeof(FlagByte) + 2*sizeof(CopyToken) + 6*sizeof(LiteralToken)
                0x82, // FlagByte = 1000 0010 (1x LiteralToken 1x CopyToken 5x LiteralToken 1x CopyToken)
                    0x66,       // LiteralToken, ASCII = f
                    0x00, 0x70, // CopyToken. Must copy "aaa"
                    0x61,       // LiteralToken, ASCII = a
                    0x67,       // LiteralToken, ASCII = g
                    0x68,       // LiteralToken, ASCII = h
                    0x69,       // LiteralToken, ASCII = i
                    0x6A,       // LiteralToken, ASCII = j
                    0x01, 0x38, // CopyToken, must copy "aaaa"

                // TokenSequence, size in bytes = 10 = 1 + 1 * sizeof(CopyToken) + 1*sizeof(LiteralToken)
                0x08, // FlagByte = 0000 1000 (3x LiteralToken 1x CopyToken 4x LiteralToken)
                    0x61,       // LiteralToken, ASCII = a
                    0x6B,       // LiteralToken, ASCII = k
                    0x6C,       // LiteralToken, ASCII = l
                    0x00, 0x30, // CopyToken, must copy "aaa"
                    0x6D,       // LiteralToken, ASCII = m
                    0x6E,       // LiteralToken, ASCII = n
                    0x6F,       // LiteralToken, ASCII = o
                    0x70,       // LiteralToken, ASCII = p

                    //////
                   
                // TokenSequence, size in bytes = 11 = 1 + 2*sizeof(CopyToken) + 6*sizeof(LiteralToken)
                0x06, // FlagByte = 0000 0110 (1x LiteralToken 2x CopyToken 5x LiteralToken)
                    0x71,       // LiteralToken, ASCII = q
                    0x02, 0x70, // CopyToken. Both CopyTokens combined must copy "aaaaaaaaaaaa"
                    0x04, 0x10, // CopyToken
                    0x72,       // LiteralToken, ASCII = r
                    0x73,       // LiteralToken, ASCII = s
                    0x74,       // LiteralToken, ASCII = t
                    0x75,       // LiteralToken, ASCII = u
                    0x76,       // LiteralToken, ASCII = v
                    
                // TokenSequence, size in bytes = 7 = 1 + 1*2 + 4*1
                0x10, // FlagByte = 0001 0000 (4x LiteralToken, 1x CopyToken)
                    0x77,       // LiteralToken, ASCII = w
                    0x78,       // LiteralToken, ASCII = x
                    0x79,       // LiteralToken, ASCII = y
                    0x7A,       // LiteralToken, ASCII = z
                    0x00, 0x3C  // CopyToken, must copy "aaa"
            };
        }
    }
}
