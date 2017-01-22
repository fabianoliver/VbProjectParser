using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;

namespace VbProjectParser.Data._PROJECTMODULES
{
    /// <summary>
    /// Page 50.
    /// Specifies that the containing MODULE Record (section 2.3.4.2.3.2) is only usable from within the current VBA project.
    /// </summary>
    public class MODULEPRIVATE : DataBase
    {
        [AutoRead(1)]
        [MustBe((UInt16)0x0028)]
        public readonly UInt16 Id;

        [AutoRead(2)]
        [MustBe((UInt32)0x00000000)]
        public readonly UInt32 Reserved;

        public MODULEPRIVATE(XlBinaryReader Data)
            : base(Data)
        {
            Validate();
        }
    }
}
