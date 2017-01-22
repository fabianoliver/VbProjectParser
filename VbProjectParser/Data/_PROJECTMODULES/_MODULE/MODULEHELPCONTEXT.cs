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
    public class MODULEHELPCONTEXT : DataBase
    {
        [AutoRead(1)]
        [MustBe((UInt16)0x001E)]
        public readonly UInt16 Id;

        [AutoRead(2)]
        [MustBe((UInt32)0x00000004)]
        public readonly UInt32 Size;

        /// <summary>
        /// An unsigned integer that specifies the Help topic identifier in the Help file specified by PROJECTHELPFILEPATH Record (section 2.3.4.2.1.7).
        /// </summary>
        [AutoRead(3)]
        public readonly UInt32 HelpContext;

        public MODULEHELPCONTEXT(XlBinaryReader Data)
            : base(Data)
        {
            Validate();
        }
    }
}
