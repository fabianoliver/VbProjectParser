using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;

namespace VbProjectParser.Data._PROJECTMODULES
{
    /// <summary>
    /// Page 48
    /// </summary>
    public class MODULEOFFSET : DataBase
    {
        [AutoRead(1)]
        [MustBe((UInt16)0x0031)]
        public readonly UInt16 Id;

        [AutoRead(2)]
        [MustBe((UInt32)0x00000004)]
        public readonly UInt32 Size;

        /// <summary>
        /// An unsigned integer that specifies the byte offset of the source code in the ModuleStream (section 2.3.4.3) named by MODULESTREAMNAME Record (section 2.3.4.2.3.2.3).
        /// </summary>
        [AutoRead(3)]
        public readonly UInt32 TextOffset;

        public MODULEOFFSET(XlBinaryReader Data)
            : base(Data)
        {
            Validate();
        }
    }
}
