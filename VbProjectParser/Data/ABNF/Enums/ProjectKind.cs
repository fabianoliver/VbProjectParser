using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data.ABNF.Enums
{
    public enum ProjectKind
    {
        /// <summary>
        /// The referenced VBA project is standalone and ProjectPath specifies a Windows file path
        /// </summary>
        Standalone_Windows,

        /// <summary>
        /// The referenced VBA project is standalone and ProjectPath specifies a Macintosh file path
        /// </summary>
        Standalone_Macintosh,

        /// <summary>
        /// The referenced VBA project is embedded and ProjectPath specifies a Windows file path
        /// </summary>
        Embedded_Windows,

        /// <summary>
        /// The refenced VBA project is embedded and ProjectPath specifies a Macintosh file path
        /// </summary>
        Embedded_Macintosh
    }


    public static class ProjectKindExtensions
    {
        public static Byte ToByte(this ProjectKind projectKind)
        {
            switch (projectKind)
            {
                case ProjectKind.Standalone_Windows:
                    return 0x41;
                case ProjectKind.Standalone_Macintosh:
                    return 0x42;
                case ProjectKind.Embedded_Windows:
                    return 0x43;
                case ProjectKind.Embedded_Macintosh:
                    return 0x44;
                default:
                    throw new NotSupportedException(String.Format("ProjectKind {0} not supported", projectKind));

            }
        }

        public static ProjectKind ToProjectKindType(this Byte @byte)
        {
            switch (@byte)
            {
                case 0x41:
                    return ProjectKind.Standalone_Windows;
                case 0x42:
                    return ProjectKind.Standalone_Macintosh;
                case 0x43:
                    return ProjectKind.Embedded_Windows;
                case 0x44:
                    return ProjectKind.Embedded_Macintosh;
                default:
                    throw new NotSupportedException(String.Format("ProjectKind byte {0} not supported", @byte));
            }
        }

        public static char ToChar(this ProjectKind projectKind)
        {
            Byte[] bytes = new byte[] { ToByte(projectKind) };
            return Encoding.ASCII.GetChars(bytes).Single();
        }
    }

}
