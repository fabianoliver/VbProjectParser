using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;

namespace VbProjectParser.Data._PROJECTINFORMATION
{
    /// <summary>
    /// Size: 10 bytes
    /// </summary>
    public class PROJECTSYSKIND : DataBase
    {
        [MustBe((UInt16)0x0001)]
        public readonly UInt16 Id;

        [MustBe((UInt32)0x00000004)]
        public readonly UInt32 Size;

        [MustBe((UInt32)0x00000000, (UInt32)0x00000001, (UInt32)0x00000002, (UInt32)0x00000003)]
        public readonly UInt32 SysKind;

        public PROJECTSYSKIND(XlBinaryReader Data)
        {
            this.Id = Data.ReadUInt16();
            this.Size = Data.ReadUInt32();
            this.SysKind = Data.ReadUInt32();

            Validate();
        }
    }
}
