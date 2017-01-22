using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data._PROJECTINFORMATION;
using VbProjectParser.Data.Exceptions;

namespace VbProjectParser.Data._PROJECTREFERENCES.ReferenceRecords
{
    public class REFERENCEREGISTERED : DataBase
    {
        [MustBe((UInt16)0x000D)]
        public readonly UInt16 Id;

        public readonly UInt32 Size;

        public readonly UInt32 SizeOfLibid;

        public Byte[] Libid;

        [MustBe((UInt32)0x00000000)]
        public readonly UInt32 Reserved1;

        [MustBe((UInt16)0x0000)]
        public readonly UInt16 Reserved2;

        protected readonly PROJECTINFORMATION ProjectInformation;

        public REFERENCEREGISTERED(XlBinaryReader Data)
        {
            this.Id = Data.ReadUInt16();
            this.Size = Data.ReadUInt32();
            this.SizeOfLibid = Data.ReadUInt32();
            this.Libid = Data.ReadBytes(this.SizeOfLibid);
            this.Reserved1 = Data.ReadUInt32();
            this.Reserved2 = Data.ReadUInt16();

            Validate();
        }

        protected override void Validate()
        {
            base.Validate();
            ValidateLibid();
        }

        protected void ValidateLibid()
        {
            if(this.Libid.Length != this.SizeOfLibid)
            {
                throw new WrongValueException("Libid.Length", this.Libid.Length, this.SizeOfLibid);
            }
        }
    }
}
