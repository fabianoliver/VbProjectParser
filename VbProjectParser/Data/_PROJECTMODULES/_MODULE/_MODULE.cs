using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data._PROJECTINFORMATION;

namespace VbProjectParser.Data._PROJECTMODULES
{
    /// <summary>
    /// Page 44
    /// </summary>
    public class MODULE : DataBase
    {
        public readonly MODULENAME NameRecord;

        [ValidateWith("ValidateNameRecords")]
        public readonly MODULENAMEUNICODE NameUnicodeRecord;

        public readonly MODULESTREAMNAME StreamNameRecord;

        public readonly MODULEDOCSTRING DocStringRecord;

        public readonly MODULEOFFSET OffsetRecord;

        public readonly MODULEHELPCONTEXT HelpContextRecord;

        public readonly MODULECOOKIE CookieRecord;

        public readonly MODULETYPE TypeRecord;

        public readonly MODULEREADONLY ReadOnlyRecord;

        public readonly MODULEPRIVATE PrivateRecord;

        [MustBe((UInt16)0x002B)]
        public readonly UInt16 Terminator;

        [MustBe((UInt32)0x00000000)]
        public readonly UInt32 Reserved;

        public bool IsPrivate
        {
            get
            {
                return this.PrivateRecord != null;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.ReadOnlyRecord != null;
            }
        }

        public MODULE(PROJECTINFORMATION ProjectInformation, XlBinaryReader Data)
        {
            this.NameRecord = new MODULENAME(ProjectInformation, Data);
            this.NameUnicodeRecord = new MODULENAMEUNICODE(Data);
            this.StreamNameRecord = new MODULESTREAMNAME(ProjectInformation, Data);
            this.DocStringRecord = new MODULEDOCSTRING(Data);
            this.OffsetRecord = new MODULEOFFSET(Data);
            this.HelpContextRecord = new MODULEHELPCONTEXT(Data);
            this.CookieRecord = new MODULECOOKIE(Data);
            this.TypeRecord = new MODULETYPE(Data);

            if(Data.PeekUInt16() == (UInt16)0x0025)
            {
                this.ReadOnlyRecord = new MODULEREADONLY(Data);
            }

            if(Data.PeekUInt16() == (UInt16)0x0028)
            {
                this.PrivateRecord = new MODULEPRIVATE(Data);
            }

            this.Terminator = Data.ReadUInt16();
            this.Reserved = Data.ReadUInt32();

            Validate();
        }

        protected ValidationResult ValidateNameRecords(object ValidationObject, MemberInfo member)
        {
            if(!this.NameRecord.GetModuleNameAsString().Equals(this.NameUnicodeRecord.GetModuleNameUnicodeAsString()))
            {
                var ex = new ArgumentException("NameUnicodeRecord.ModuleNameUnicode (string) was not equal NameRecord.ModuleName (string)", "NameUnicordRecord");
                return new ValidationResult(ex);
            }

            return new ValidationResult();
        }
    }
}
