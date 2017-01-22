using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;

namespace VbProjectParser.Data._PROJECTMODULES
{
    /// <summary>
    /// Page 49.
    /// Specifies that the containing MODULE Record (section 2.3.4.2.3.2) is read-only.
    /// </summary>
    public class MODULEREADONLY : DataBase
    {
        [AutoRead(1)]
        [MustBe((UInt16)0x0025)]
        public readonly UInt16 Id;

        [AutoRead(2)]
        [MustBe((UInt32)0x00000000)]
        public readonly UInt32 Reserved;

        public MODULEREADONLY(XlBinaryReader Data)
            : base(Data)
        {
            Validate();
        }
    }
}
