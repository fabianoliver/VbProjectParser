using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Compression
{
    
    public abstract class Token
    {
        //public abstract Byte[] GetDataInRawBytes();
        public abstract int GetSizeInBytes();
        public abstract void Decompress(DecompressedBuffer buffer, DecompressionState state);
    }
    
    public class TokenSequence
    {
        public enum TokenType : byte
        {
            LiteralToken = 0x00,
            CopyToken = 0x01
        }

        public readonly Byte FlagByte;
        protected readonly IList<Token> _Tokens = new List<Token>();

        public TokenSequence(XlBinaryReader CompressedData, int remainingBytes)
        {
            this.FlagByte = CompressedData.ReadByte();
            --remainingBytes;
 
            for (int i = 0; i < 8; i++)
            {
                if (remainingBytes == 0)
                    break;

                //int index = (i - 7) * (-1); // The most significant byte describes the first token. The second most significant byte the second token. etc. So we map 0 -> 7,   1 -> 6,   2 -> 5,   3 -> 4,   4 -> 3,   5 -> 2,   6 -> 1,   7 -> 0
                TokenType tokenType = GetTokenTypeAtIndex(i);

                if(tokenType == TokenType.CopyToken)
                {
                    var token = new CopyToken(CompressedData);
                    this._Tokens.Add(token);
                    remainingBytes -= token.GetSizeInBytes();
                } else if(tokenType == TokenType.LiteralToken)
                {
                    var token = new LiteralToken(CompressedData);
                    this._Tokens.Add(token);
                    remainingBytes -= token.GetSizeInBytes();
                }
                else {
                    throw new Exception();
                }

                
                // todo: last token sequence could contain less than 8 tokens

            }
        }

        public int GetSizeInBytes()
        {
            // FlagByte + size of tokens
            return sizeof(Byte) + _Tokens.Sum(token => token.GetSizeInBytes());
        }

        /*
        public Byte[] GetDataInRawBytes()
        {
            IEnumerable<Byte> result = new Byte[] { this.FlagByte };
            foreach(var token in _Tokens)
            {
                result = result.Concat(token.GetDataInRawBytes());
            }

            return result.ToArray();
        }*/

        public void Decompress(DecompressedBuffer buffer, DecompressionState state)
        {
            ++state.CompressedCurrent;

            foreach(var token in this._Tokens)
            {
                token.Decompress(buffer, state);
            }
        }

        /*
        public void Decompress(IList<Byte> Target)
        {
            for (int i = 0; i < this._Tokens.Count; i++)
            {
                var token = _Tokens[i];
                token.Decompress(Target, i);
            }
        }*/

        protected TokenType GetTokenTypeAtIndex(int index)
        {
            return (FlagByte.ReadBitAtPosition(index) == true) ? TokenType.CopyToken : TokenType.LiteralToken;
        }
    }
}
