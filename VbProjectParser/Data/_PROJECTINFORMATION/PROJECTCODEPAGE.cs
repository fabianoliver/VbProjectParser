using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;

namespace VbProjectParser.Data._PROJECTINFORMATION
{
    public class PROJECTCODEPAGE : DataBase
    {
        [MustBe((UInt16)0x0003)]
        public readonly UInt16 Id;

        [MustBe((UInt32)0x00000002)]
        public readonly UInt32 Size;

        public readonly UInt16 CodePage;

        public PROJECTCODEPAGE(XlBinaryReader Data)
        {
            this.Id = Data.ReadUInt16();
            this.Size = Data.ReadUInt32();
            this.CodePage = Data.ReadUInt16();

            Validate();
        }

        /// <summary>
        /// Gets the Encoding object as specified by this record
        /// </summary>
        public Encoding GetEncoding()
        {
            return Encoding.GetEncoding(Convert.ToInt32(this.CodePage));
        }
    }
}
