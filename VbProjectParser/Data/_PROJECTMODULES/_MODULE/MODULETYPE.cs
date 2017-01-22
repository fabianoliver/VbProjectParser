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
    public class MODULETYPE : DataBase
    {
        /// <summary>
        /// An unsigned integer that specifies the identifier for this record. MUST be 0x0021 when the containing MODULE Record (section 2.3.4.2.3.2) is a procedural module. MUST be 0x0022 when the containing MODULE Record (section 2.3.4.2.3.2) is a document module, class module, or designer module.
        /// </summary>
        [AutoRead(1)]
        [MustBe((UInt16)0x0021, (UInt16)0x0022)]
        public readonly UInt16 Id;

        [AutoRead(2)]
        [MustBe((UInt32)0x00000000)]
        public readonly UInt32 Size;

        public MODULETYPE(XlBinaryReader Data)
            : base(Data)
        {
            Validate();
        }
    }
}
