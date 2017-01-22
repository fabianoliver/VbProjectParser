using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;

namespace VbProjectParser.Data._PROJECTMODULES
{
    public class PROJECTCOOKIE : DataBase
    {
        [MustBe((UInt16)0x0013)]
        public readonly UInt16 Id;

        [MustBe((UInt32)0x00000002)]
        public readonly UInt32 Size;

        /// <summary>
        /// Must be 0xFFFF on write
        /// </summary>
        public readonly UInt16 Cookie;

        public PROJECTCOOKIE(XlBinaryReader Data)
        {
            this.Id = Data.ReadUInt16();
            this.Size = Data.ReadUInt32();
            this.Cookie = Data.ReadUInt16();

            Validate();
        }
    }
}
