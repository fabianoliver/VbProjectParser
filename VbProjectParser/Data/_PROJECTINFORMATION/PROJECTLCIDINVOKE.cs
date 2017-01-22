using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;

namespace VbProjectParser.Data._PROJECTINFORMATION
{
    public class PROJECTLCIDINVOKE : DataBase
    {
        [MustBe((UInt16)0x0014)]
        public readonly UInt16 Id;

        [MustBe((UInt32)0x00000004)]
        public readonly UInt32 Size;

        [MustBe((UInt32)0x00000409)]
        public readonly UInt32 LcidInvoke;

        public PROJECTLCIDINVOKE(XlBinaryReader Data)
        {
            this.Id = Data.ReadUInt16();
            this.Size = Data.ReadUInt32();
            this.LcidInvoke = Data.ReadUInt32();

            Validate();
        }
    }
}
