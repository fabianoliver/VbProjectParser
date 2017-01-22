using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;

namespace VbProjectParser.Data._PROJECTINFORMATION
{
    public class PROJECTLIBFLAGS : DataBase
    {
        [MustBe((UInt16)0x0008)]
        public readonly UInt16 Id;

        [MustBe((UInt32)0x00000004)]
        public readonly UInt32 Size;

        [MustBe((UInt32)0x00000000)]
        public readonly UInt32 ProjectLibFlags;

        public PROJECTLIBFLAGS(XlBinaryReader Data)
        {
            this.Id = Data.ReadUInt16();
            this.Size = Data.ReadUInt32();
            this.ProjectLibFlags = Data.ReadUInt32();

            Validate();
        }
    }
}
