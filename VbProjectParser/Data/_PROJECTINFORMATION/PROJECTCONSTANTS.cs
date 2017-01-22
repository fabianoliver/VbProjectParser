using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data.Exceptions;

namespace VbProjectParser.Data._PROJECTINFORMATION
{
    public class PROJECTCONSTANTS : DataBase
    {
        [MustBe((UInt16)0x000C)]
        public readonly UInt16 Id;

        [Range(UInt32.MinValue, 1015)]
        public readonly UInt32 SizeOfConstants;

        [LengthMustEqualMember("SizeOfConstants")]
        public readonly Byte[] Constants;

        [MustBe((UInt16)0x003C)]
        public readonly UInt16 Reserved;

        public readonly UInt32 SizeOfConstantsUnicode;

        [LengthMustEqualMember("SizeOfConstantsUnicode")]
        [ValidateWith("ValidateCompareConstants")]
        public readonly Byte[] ConstantsUnicode;

        protected readonly PROJECTINFORMATION parent;

        public PROJECTCONSTANTS(PROJECTINFORMATION parent, XlBinaryReader Data)
        {
            this.parent = parent;

            this.Id = Data.ReadUInt16();
            this.SizeOfConstants = Data.ReadUInt32();
            this.Constants = Data.ReadBytes(this.SizeOfConstants);
            this.Reserved = Data.ReadUInt16();
            this.SizeOfConstantsUnicode = Data.ReadUInt32();
            this.ConstantsUnicode = Data.ReadBytes(this.SizeOfConstantsUnicode);

            Validate();
        }

        protected ValidationResult ValidateCompareConstants(object ValidationObject, MemberInfo member)
        {
            if(!GetConstantsAsString().Equals(GetConstantsUnicodeAsString()))
            {
                var ex = new ArgumentException("ConstantsUnicode (string) did not equals Constants (string)", "ConstantsUnicode");
                return new ValidationResult(ex);
            }

            return new ValidationResult();
        }

        public string GetConstantsAsString(Encoding encoding)
        {
            return encoding.GetString(this.Constants);
        }

        public string GetConstantsAsString(PROJECTCODEPAGE Codepage)
        {
            return GetConstantsAsString(Codepage.GetEncoding());
        }

        public string GetConstantsAsString(PROJECTINFORMATION ProjectInformation)
        {
            return GetConstantsAsString(ProjectInformation.CodePageRecord);
        }

        public string GetConstantsAsString()
        {
            return GetConstantsAsString(this.parent);
        }

        public string GetConstantsUnicodeAsString()
        {
            return Encoding.Unicode.GetString(this.ConstantsUnicode);
        }
    }
}
