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
    public class MODULEDOCSTRING : DataBase
    {
        [AutoRead(1)]
        [MustBe((UInt16)0x001C)]
        public readonly UInt16 Id;

        [AutoRead(2)]
        public readonly UInt32 SizeOfDocString;

        [AutoRead(3, "SizeOfDocString")]
        [LengthMustEqualMember("SizeOfDocString")]
        public readonly Byte[] DocString;

        [AutoRead(4)]
        [MustBe((UInt16)0x0048)]
        public readonly UInt16 Reserved;

        [AutoRead(5)]
        [IsEvenNumber]
        public readonly UInt32 SizeOfDocStringUnicode;

        [AutoRead(6,"SizeOfDocStringUnicode")]
        [LengthMustEqualMember("SizeOfDocStringUnicode")]
        public readonly Byte[] DocStringUnicode;

        public MODULEDOCSTRING(XlBinaryReader Data)
            : base(Data)
        {
            Validate();
        }
    }
}
