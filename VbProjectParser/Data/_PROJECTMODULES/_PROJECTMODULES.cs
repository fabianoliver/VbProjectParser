using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data._PROJECTINFORMATION;

namespace VbProjectParser.Data._PROJECTMODULES
{
    public class PROJECTMODULES : DataBase
    {
        /// <summary>
        /// An unsigned integer that specifies the identifier for this record. MUST be 0x000F.
        /// </summary>
        [MustBe((UInt16)0x000F)]
        public readonly UInt16 Id;

        /// <summary>
        /// An unsigned integer that specifies the size of Count. MUST be 0x00000002.
        /// </summary>
        [MustBe((UInt32)0x00000002)]
        public readonly UInt32 Size;

        /// <summary>
        /// An unsigned integer that specifies the number of elements in Modules.
        /// </summary>
        public UInt16 Count { get; internal set; }

        /// <summary>
        /// A PROJECTCOOKIE Record (section 2.3.4.2.3.1).
        /// </summary>
        public readonly PROJECTCOOKIE ProjectCookieRecord;

        /// <summary>
        /// A PROJECTCOOKIE Record (section 2.3.4.2.3.1). Contains $Count elements.
        /// </summary>
        [LengthMustEqualMember("Count")]
        public readonly MODULE[] Modules;

        public PROJECTMODULES(PROJECTINFORMATION ProjectInformation, XlBinaryReader Data)
        {
            this.Id = Data.ReadUInt16();
            this.Size = Data.ReadUInt32();
            this.Count = Data.ReadUInt16();
            this.ProjectCookieRecord = new PROJECTCOOKIE(Data);

            // Read Modules
            this.Modules = new MODULE[this.Count];
            for (int i = 0; i < this.Count; i++)
            {
                var module = new MODULE(ProjectInformation, Data);
                this.Modules[i] = module;

            }

            Validate();
        }
    }
}
