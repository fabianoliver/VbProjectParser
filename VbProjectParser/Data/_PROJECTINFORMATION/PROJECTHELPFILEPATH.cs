using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data.Exceptions;

namespace VbProjectParser.Data._PROJECTINFORMATION
{
    public class PROJECTHELPFILEPATH : DataBase
    {
        [MustBe((UInt16)0x0006)]
        public readonly UInt16 Id;

        [Range(UInt32.MinValue, 260)]
        public readonly UInt32 SizeOfHelpFile1;

        [LengthMustEqualMember("SizeOfHelpFile1")]
        public readonly Byte[] HelpFile1;

        [MustBe((UInt16)0x003D)]
        public readonly UInt16 Reserved;

        [ValueMustEqualMember("SizeOfHelpFile1")]
        public readonly UInt32 SizeOfHelpFile2;

        [LengthMustEqualMember("SizeOfHelpFile2")]
        public readonly Byte[] HelpFile2;

        protected PROJECTINFORMATION parent;

        public PROJECTHELPFILEPATH(PROJECTINFORMATION parent, XlBinaryReader Data)
        {
            this.parent = parent;

            this.Id = Data.ReadUInt16();
            this.SizeOfHelpFile1 = Data.ReadUInt32();
            this.HelpFile1 = Data.ReadBytes(this.SizeOfHelpFile1);
            this.Reserved = Data.ReadUInt16();
            this.SizeOfHelpFile2 = Data.ReadUInt32();
            this.HelpFile2 = Data.ReadBytes(this.SizeOfHelpFile2);

            Validate();
        }



        public string GetHelpFile1AsString(Encoding encoding)
        {
            return encoding.GetString(this.HelpFile1);
        }

        public string GetHelpFile1AsString(PROJECTCODEPAGE Codepage)
        {
            return GetHelpFile1AsString(Codepage.GetEncoding());
        }

        public string GetHelpFile1AsString(PROJECTINFORMATION ProjectInformation)
        {
            return GetHelpFile1AsString(ProjectInformation.CodePageRecord);
        }

        public string GetHelpFile1AsString()
        {
            return GetHelpFile1AsString(this.parent);
        }
    }
}
