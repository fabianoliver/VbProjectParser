using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data.ABNF.Enums
{
    public enum LibidReferenceKind
    {
        /// <summary>
        /// Windows file path
        /// </summary>
        WindowsFilePath,

        /// <summary>
        /// Macintosh file path
        /// </summary>
        MacintoshFilePath
    }

    
    public static class LibidReferenceKindKindExtensions
    {
        public static Byte ToByte(this LibidReferenceKind LibidReferenceKind)
        {
            switch (LibidReferenceKind)
            {
                case LibidReferenceKind.WindowsFilePath:
                    return 0x47;
                case LibidReferenceKind.MacintoshFilePath:
                    return 0x48;
                default:
                    throw new NotSupportedException(String.Format("LibidReferenceKind {0} not supported", LibidReferenceKind));

            }
        }

        public static LibidReferenceKind ToLibidReferenceKindType(this Byte @byte)
        {
            switch (@byte)
            {
                case 0x47:
                    return LibidReferenceKind.WindowsFilePath;
                case 0x48:
                    return LibidReferenceKind.MacintoshFilePath;
                default:
                    throw new NotSupportedException(String.Format("LibidReferenceKind byte {0} not supported", @byte));
            }
        }

        public static char ToChar(this LibidReferenceKind LibidReferenceKind)
        {
            Byte[] bytes = new byte[] { ToByte(LibidReferenceKind) };
            return Encoding.ASCII.GetChars(bytes).Single();
        }
    }
}
