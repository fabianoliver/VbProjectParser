using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data.Exceptions;

namespace VbProjectParser.Data._PROJECTINFORMATION
{
    public class PROJECTCOMPATVERSION : DataBase
    {
        public static UInt16 RecordId = 0x004A;

        [MustBe((UInt16)0x004A)]
        public readonly UInt16 Id;

        [MustBe((UInt32)0x00000004)]
        public readonly UInt32 Size;

        public readonly UInt32 CompatVersion;

        public PROJECTCOMPATVERSION(XlBinaryReader Data)
        {
            this.Id = Data.ReadUInt16();
            this.Size = Data.ReadUInt32();
            this.CompatVersion = Data.ReadUInt32();

            Validate();
        }
    }
}
