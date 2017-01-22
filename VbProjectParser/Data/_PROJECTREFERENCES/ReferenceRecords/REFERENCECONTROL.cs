using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data._PROJECTINFORMATION;

namespace VbProjectParser.Data._PROJECTREFERENCES.ReferenceRecords
{
    // page 41
    // todo: the specification for this one is weird...
    public class REFERENCECONTROL : DataBase
    {
        [MustBe((UInt16)0x002F)]
        public readonly UInt16 Id;

        public readonly UInt32 SizeTwiddled;

        public readonly UInt32 SizeOfLibidTwiddled;

        [LengthMustEqualMember("SizeOfLibidTwiddled")]
        public readonly Byte[] LibidTwiddled;

        [MustBe((UInt32)0x00000000)]
        public readonly UInt32 Reserved1;

        [MustBe((UInt16)0x0000)]
        public readonly UInt16 Reserved2;

        /// <summary>
        /// A REFERENCENAME Record (section 2.3.4.2.2.2) that specifies the name of the extended type library. This field is optional (= can be null)
        /// </summary>
        public readonly REFERENCENAME NameRecordExtended;

        [MustBe((UInt16)0x0030)]
        public readonly UInt16 Reserved3;

        [ValidateWith("ValidateSizeExtended")]
        public readonly UInt32 SizeExtended;

        public readonly UInt32 SizeOfLibidExtended;

        [LengthMustEqualMember("SizeOfLibidExtended")]
        public readonly Byte[] LibidExtended;

        [MustBe((UInt32)0x00000000)]
        public readonly UInt32 Reserved4;

        [MustBe((UInt16)0x0000)]
        public readonly UInt16 Reserved5;

        public readonly Guid OriginalTypeLib;

        public readonly UInt32 Cookie;

        public REFERENCECONTROL(PROJECTINFORMATION ProjectInformation, XlBinaryReader Data)
        {
            this.Id = Data.ReadUInt16();
            this.SizeTwiddled = Data.ReadUInt32();
            this.SizeOfLibidTwiddled = Data.ReadUInt32();
            this.LibidTwiddled = Data.ReadBytes(this.SizeOfLibidTwiddled);
            this.Reserved1 = Data.ReadUInt32();
            this.Reserved2 = Data.ReadUInt16();

            UInt16 peek = Data.PeekUInt16();
            if(peek == (UInt16)0x0016)
            {
                // REFERENCENAME record
                this.NameRecordExtended = new REFERENCENAME(ProjectInformation, Data);
            }

            this.Reserved3 = Data.ReadUInt16();
            this.SizeExtended = Data.ReadUInt32();
            this.SizeOfLibidExtended = Data.ReadUInt32();
            this.LibidExtended = Data.ReadBytes(this.SizeOfLibidExtended);
            this.Reserved4 = Data.ReadUInt32();
            this.Reserved5 = Data.ReadUInt16();
            this.OriginalTypeLib = Data.ReadGuid();
            this.Cookie = Data.ReadUInt32();

            Validate();
        }

        /// <summary>
        /// SizeExnteded must be sum of the size in bytes of SizeOfLibidExtended, LibidExtended, Reserved4, Reserved5, OriginalTypeLib, and Cookie.
        /// </summary>
        protected ValidationResult ValidateSizeExtended(object ValidationObject, MemberInfo member)
        {
            var t = sizeof(UInt32) +        // size of SizeOfLibidExtended
                    (uint)LibidExtended.Length +
                    sizeof(UInt32) +        // Reserved4
                    sizeof(UInt16) +        // Reserved5
                    (uint)16 +          // OriginalTypeLib
                    sizeof(UInt32);         // Cookie

            if(this.SizeExtended != t)
            {
                var ex = new ArgumentException(String.Format("SizeExtended expected size {0}, but was {1}", t, this.SizeExtended), "SizeExtended");
                return new ValidationResult(ex);
            }

            return new ValidationResult();
        }
    }
}
