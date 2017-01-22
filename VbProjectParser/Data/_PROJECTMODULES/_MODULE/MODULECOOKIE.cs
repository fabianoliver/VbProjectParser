using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;

namespace VbProjectParser.Data._PROJECTMODULES
{
    /// <summary>
    /// Page 49
    /// </summary>
    public class MODULECOOKIE : DataBase
    {
        [AutoRead(1)]
        [MustBe((UInt16)0x002C)]
        public readonly UInt16 Id;

        [AutoRead(2)]
        [MustBe((UInt32)0x00000002)]
        public readonly UInt32 Size;

        [AutoRead(3)]
        public readonly UInt16 Cookie;

        public MODULECOOKIE(XlBinaryReader Data)
            : base(Data)
        {
            Validate();
        }
    }
}
