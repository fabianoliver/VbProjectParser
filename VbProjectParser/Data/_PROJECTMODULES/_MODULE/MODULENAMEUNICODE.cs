using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data._PROJECTINFORMATION;

namespace VbProjectParser.Data._PROJECTMODULES
{
    public class MODULENAMEUNICODE : DataBase
    {
        [MustBe((UInt16)0x0047)]
        public readonly UInt16 Id;

        [IsEvenNumber]
        public readonly UInt32 SizeOfModuleNameUnicode;

        [LengthMustEqualMember("SizeOfModuleNameUnicode")]
        public readonly Byte[] ModuleNameUnicode;

        public MODULENAMEUNICODE(XlBinaryReader Data)
        {
            this.Id = Data.ReadUInt16();
            this.SizeOfModuleNameUnicode = Data.ReadUInt32();
            this.ModuleNameUnicode = Data.ReadBytes(this.SizeOfModuleNameUnicode);

            Validate();
        }

        public string GetModuleNameUnicodeAsString()
        {
            return Encoding.Unicode.GetString(this.ModuleNameUnicode);
        }
    }
}
