using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;

namespace VbProjectParser.Data._PROJECTINFORMATION
{
    public class PROJECTHELPCONTEXT : DataBase
    {
        [MustBe((UInt16)0x0007)]
        public readonly UInt16 Id;

        [MustBe((UInt32)0x00000004)]
        public readonly UInt32 Size;

        public readonly UInt32 HelpContext;

        public PROJECTHELPCONTEXT(XlBinaryReader Data)
        {
            this.Id = Data.ReadUInt16();
            this.Size = Data.ReadUInt32();
            this.HelpContext = Data.ReadUInt32();

            Validate();
        }
    }
}
