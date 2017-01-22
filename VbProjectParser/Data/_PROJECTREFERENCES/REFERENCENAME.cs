using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data._PROJECTINFORMATION;
using VbProjectParser.Data.Exceptions;

namespace VbProjectParser.Data._PROJECTREFERENCES
{
    /// <summary>
    /// Page 38
    /// </summary>
    public class REFERENCENAME : DataBase
    {
        [MustBe((UInt16)0x0016)]
        public readonly UInt16 Id;

        public readonly UInt32 SizeOfName;

        public readonly Byte[] Name;

        [MustBe((UInt16)0x003E)]
        public readonly UInt16 Reserved;

        public readonly UInt32 SizeOfNameUnicode;

        public readonly Byte[] NameUnicode;

        // TODO: RefProjectName, RefLibararyName, Reserved, SizeOfNameUnicode, NameUnicode

        protected readonly PROJECTINFORMATION ProjectInformation;

        public REFERENCENAME(PROJECTINFORMATION ProjectInformation, XlBinaryReader Data)
        {
            this.ProjectInformation = ProjectInformation;

            this.Id = Data.ReadUInt16();
            this.SizeOfName = Data.ReadUInt32();
            this.Name = Data.ReadBytes(this.SizeOfName);
            this.Reserved = Data.ReadUInt16();
            this.SizeOfNameUnicode = Data.ReadUInt32();
            this.NameUnicode = Data.ReadBytes(this.SizeOfNameUnicode);

            Validate();
        }

        protected override void Validate()
        {
            base.Validate();
            ValidateName();
            ValidateNameUnicode();
            ValidateCompareNames();
        }

        protected void ValidateName()
        {
            if(this.Name.Length != this.SizeOfName)
            {
                throw new WrongValueException("Name.Length", this.Name.Length, this.SizeOfName);
            }
        }

        protected void ValidateNameUnicode()
        {
            if(this.NameUnicode.Length != this .SizeOfNameUnicode)
            {
                throw new WrongValueException("NameUnicode", this.NameUnicode.Length, this.SizeOfNameUnicode);
            }
        }

        protected void ValidateCompareNames()
        {
            if(!this.GetNameAsString().Equals(this.GetNameUnicodeAsString()))
            {
                throw new WrongValueException("NameUnicode vs. Name", this.NameUnicode, this.Name);
            }
        }

        public string GetNameAsString(Encoding encoding)
        {
            return encoding.GetString(this.Name);
        }

        public string GetNameAsString(PROJECTCODEPAGE Codepage)
        {
            return GetNameAsString(Codepage.GetEncoding());
        }

        public string GetNameAsString(PROJECTINFORMATION ProjectInformation)
        {
            return GetNameAsString(ProjectInformation.CodePageRecord);
        }

        public string GetNameAsString()
        {
            return GetNameAsString(this.ProjectInformation);
        }

        public string GetNameUnicodeAsString()
        {
            return Encoding.Unicode.GetString(this.NameUnicode);
        }
    }
}
