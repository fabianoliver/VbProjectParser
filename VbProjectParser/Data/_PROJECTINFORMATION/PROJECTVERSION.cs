using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;

namespace VbProjectParser.Data._PROJECTINFORMATION
{
    public class PROJECTVERSION : DataBase
    {
        [MustBe((UInt16)0x0009)]
        public readonly UInt16 Id;

        [MustBe((UInt32)0x00000004)]
        public readonly UInt32 Reserved;

        public readonly UInt32 VersionMajor;

        public readonly UInt16 VersionMinor;

        public PROJECTVERSION(XlBinaryReader Data)
        {
            this.Id = Data.ReadUInt16();
            this.Reserved = Data.ReadUInt32();
            this.VersionMajor = Data.ReadUInt32();
            this.VersionMinor = Data.ReadUInt16();

            Validate();
        }
    }
}
