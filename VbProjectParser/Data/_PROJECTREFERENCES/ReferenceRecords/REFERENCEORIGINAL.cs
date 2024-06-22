using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;

namespace VbProjectParser.Data._PROJECTREFERENCES.ReferenceRecords
{
    public class REFERENCEORIGINAL : DataBase
    {
        [MustBe((UInt16)0x0033)]
        public readonly UInt16 Id;

        public readonly UInt32 SizeOfLibidOriginal;

        [LengthMustEqualMember("SizeOfLibidOriginal")]
        public Byte[] LibidOriginal;

        public REFERENCEORIGINAL(XlBinaryReader Data)
        {
            this.Id = Data.ReadUInt16();
            this.SizeOfLibidOriginal = Data.ReadUInt32();
            this.LibidOriginal = Data.ReadBytes(this.SizeOfLibidOriginal);

            Validate();
        }
    }
}
