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
    public class PROJECTDOCSTRING : DataBase
    {
        [MustBe((UInt16)0x0005)]
        public readonly UInt16 Id;

        [Range(UInt32.MinValue, 2000)]
        public readonly UInt32 SizeOfDocString;

        [LengthMustEqualMember("SizeOfDocString")]
        public readonly Byte[] DocString;

        [MustBe((UInt16)0x0040)]
        public readonly UInt16 Reserved;

        [IsEvenNumber]
        public readonly UInt32 SizeOfDocStringUnicode;

        [LengthMustEqualMember("SizeOfDocStringUnicode")]
        [ValidateWith("ValidateBothDocStrings")]
        public readonly Byte[] DocStringUnicode;

        protected readonly PROJECTINFORMATION parent;

        public PROJECTDOCSTRING(PROJECTINFORMATION parent, XlBinaryReader Data)
        {
            this.parent = parent;

            this.Id = Data.ReadUInt16();
            this.SizeOfDocString = Data.ReadUInt32();
            this.DocString = Data.ReadBytes(Convert.ToInt32(this.SizeOfDocString));
            this.Reserved = Data.ReadUInt16();
            this.SizeOfDocStringUnicode = Data.ReadUInt32();
            this.DocStringUnicode = Data.ReadBytes(Convert.ToInt32(this.SizeOfDocStringUnicode));

            Validate();
        }

  
        protected ValidationResult ValidateBothDocStrings(object ValidationObject, MemberInfo member)
        {
            if(!GetDocStringAsString().Equals(GetDocStringUnicodeAsString()))
            {
                var ex = new ArgumentException("DocStringUnicode (string) was not equal DocString (string)", "DocStringUnicde");
                return new ValidationResult(ex);
            }

            return new ValidationResult();
        }

        public string GetDocStringAsString(Encoding encoding)
        {
            return encoding.GetString(this.DocString);
        }

        public string GetDocStringAsString(PROJECTCODEPAGE Codepage)
        {
            return GetDocStringAsString(Codepage.GetEncoding());
        }

        public string GetDocStringAsString(PROJECTINFORMATION ProjectInformation)
        {
            return GetDocStringAsString(ProjectInformation.CodePageRecord);
        }

        public string GetDocStringAsString()
        {
            return GetDocStringAsString(this.parent);
        }

        public string GetDocStringUnicodeAsString()
        {
            return Encoding.Unicode.GetString(this.DocStringUnicode);
        }

    }
}
