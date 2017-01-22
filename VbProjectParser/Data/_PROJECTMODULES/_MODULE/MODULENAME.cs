using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data._PROJECTINFORMATION;

namespace VbProjectParser.Data._PROJECTMODULES
{
    public class MODULENAME : DataBase
    {
        [AutoRead(1)]
        [MustBe((UInt16)0x0019)]
        public readonly UInt16 Id;

        [AutoRead(2)]
        public readonly UInt32 SizeOfModuleName;

        [AutoRead(3, "SizeOfModuleName")]
        [LengthMustEqualMember("SizeOfModuleName")]
        public readonly Byte[] ModuleName;

        protected readonly PROJECTINFORMATION ProjectInformation;

        public MODULENAME(PROJECTINFORMATION ProjectInformation, XlBinaryReader Data)
            : base(Data)
        {
            this.ProjectInformation = ProjectInformation;

            //this.Id = Data.ReadUInt16();
            //this.SizeOfModuleName = Data.ReadUInt32();
            //this.ModuleName = Data.ReadBytes(this.SizeOfModuleName);

            Validate();
        }

        public string GetModuleNameAsString(Encoding encoding)
        {
            return encoding.GetString(this.ModuleName);
        }

        public string GetModuleNameAsString(PROJECTCODEPAGE Codepage)
        {
            return GetModuleNameAsString(Codepage.GetEncoding());
        }

        public string GetModuleNameAsString(PROJECTINFORMATION ProjectInformation)
        {
            return GetModuleNameAsString(ProjectInformation.CodePageRecord);
        }

        public string GetModuleNameAsString()
        {
            return GetModuleNameAsString(this.ProjectInformation);
        }
    }
}
