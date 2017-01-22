using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data.Exceptions;

namespace VbProjectParser.Data._PROJECTINFORMATION
{
    public class PROJECTNAME : DataBase
    {
        [MustBe((UInt16)0x0004)]
        public readonly UInt16 Id;

        [Range(1, 128)]
        public readonly UInt32 Size;

        [LengthMustEqualMember("Size")]
        public readonly Byte[] ProjectName;

        protected readonly PROJECTINFORMATION parent;

        public PROJECTNAME(PROJECTINFORMATION parent, XlBinaryReader Data)
        {
            this.parent = parent;

            this.Id = Data.ReadUInt16();
            this.Size = Data.ReadUInt32();
            this.ProjectName = Data.ReadBytes((int)this.Size);

            Validate();
        }

        /// <summary>
        /// Returns a string of the Project Name, where encoding needs to be the project's
        /// Encoding (as specified in the PROJECTCODEPAGE record)
        /// </summary>
        public string GetProjectNameAsString(Encoding encoding)
        {
            return encoding.GetString(this.ProjectName);
        }

        public string GetProjectNameAsString(PROJECTCODEPAGE Codepage)
        {
            return GetProjectNameAsString(Codepage.GetEncoding());
        }

        public string GetProjectNameAsString(PROJECTINFORMATION ProjectInformation)
        {
            return GetProjectNameAsString(ProjectInformation.CodePageRecord);
        }

        public string GetProjectNameAsString()
        {
            return GetProjectNameAsString(this.parent);
        }
    }
}
