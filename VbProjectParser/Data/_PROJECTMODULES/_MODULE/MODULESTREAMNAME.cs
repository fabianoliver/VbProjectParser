using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data._PROJECTINFORMATION;

namespace VbProjectParser.Data._PROJECTMODULES
{
    public class MODULESTREAMNAME : DataBase
    {
        [MustBe((UInt16)0x001A)]
        public readonly UInt16 Id;

        public readonly UInt32 SizeOfStreamName;

        [LengthMustEqualMember("SizeOfStreamName")]
        public readonly Byte[] StreamName;

        [MustBe((UInt16)0x0032)]
        public readonly UInt16 Reserved;

        public readonly UInt32 SizeOfStreamNameUnicode;

        [LengthMustEqualMember("SizeOfStreamNameUnicode")]
        public readonly Byte[] StreamNameUnicode;

        protected readonly PROJECTINFORMATION ProjectInformation;

        public MODULESTREAMNAME(PROJECTINFORMATION ProjectInformation, XlBinaryReader Data)
        {
            this.ProjectInformation = ProjectInformation;

            this.Id = Data.ReadUInt16();
            this.SizeOfStreamName = Data.ReadUInt32();
            this.StreamName = Data.ReadBytes(this.SizeOfStreamName);
            this.Reserved = Data.ReadUInt16();
            this.SizeOfStreamNameUnicode = Data.ReadUInt32();
            this.StreamNameUnicode = Data.ReadBytes(this.SizeOfStreamNameUnicode);

            Validate();
        }

        public string GetStreamNameAsString(Encoding encoding)
        {
            return encoding.GetString(this.StreamName);
        }

        public string GetStreamNameAsString(PROJECTCODEPAGE Codepage)
        {
            return GetStreamNameAsString(Codepage.GetEncoding());
        }

        public string GetStreamNameAsString(PROJECTINFORMATION ProjectInformation)
        {
            return GetStreamNameAsString(ProjectInformation.CodePageRecord);
        }

        public string GetStreamNameAsString()
        {
            return GetStreamNameAsString(this.ProjectInformation);
        }

        public string GetStreamNameUnicodeAsString()
        {
            return Encoding.Unicode.GetString(this.StreamNameUnicode);
        }
    }
}
