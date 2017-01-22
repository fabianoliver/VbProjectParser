using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;

namespace VbProjectParser.Data
{
    /// <summary>
    /// Section 2.3.4.1, page 29
    /// 
    /// The _VBA_PROJECT stream contains the version-dependent description of a VBA project.
    /// The first seven bytes of the stream are version-independent and therefore can be read by any version.
    /// </summary>
    public class _VBA_PROJECTStream : DataBase
    {
        /// <summary>
        /// Reserved1 (2 bytes): MUST be 0x61CC. MUST be ignored.
        /// </summary>
        [MustBe((UInt16)0x61CC)]
        public UInt16 Reserved1 { get; private set; }

        /// <summary>
        /// Version (2 bytes): An unsigned integer that specifies the version of VBA used to create the VBA project. MUST be ignored on read. MUST be 0xFFFF on write.
        /// </summary>
        public UInt16 Version { get; private set; }

        /// <summary>
        /// Reserved2 (1 byte): MUST be 0x00. MUST be ignored.
        /// </summary>
        [MustBe((Byte)0x00)]
        public Byte Reserved2 { get; private set; }

        /// <summary>
        /// Reserved3 (2 bytes): Undefined. MUST be ignored.
        /// </summary>
        public UInt16 Reserved3 { get; set; }

        public Byte[] PerformanceCache { get; private set; }

        public _VBA_PROJECTStream(XlBinaryReader data)
        {
            this.Reserved1 = data.ReadUInt16();
            this.Version = data.ReadUInt16();
            this.Reserved2 = data.ReadByte();
            this.Reserved3 = data.ReadUInt16();

            int PerformanceCacheLength = data.Length - 7;
            this.PerformanceCache = data.ReadBytes(PerformanceCacheLength);

            Validate();
        }

        internal _VBA_PROJECTStream()
        {
            this.Reserved1 = 0x61CC;
            this.Version = 0xFFFF;
            this.Reserved2 = 0x00;
            this.Reserved3 = 0x0000; // undefined. we'll just zero it out.
            this.PerformanceCache = new Byte[] { };
        }
    }
}
