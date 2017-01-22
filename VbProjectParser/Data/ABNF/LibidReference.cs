using AbnfFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Data.ABNF.Enums;

namespace VbProjectParser.Data.ABNF
{
    public class LibidReference
    {
        public LibidReferenceKind LibidReferenceKind { get; set; }

        /// <summary>
        /// The GUID of the Automation type library
        /// </summary>
        public Guid LibidGuid { get; set; }

        /// <summary>
        /// An unsigned integer that specifies the major version of the Automation type library
        /// </summary>
        public uint LibidMajorVersion { get; set; }

        /// <summary>
        /// An unsigned integer that specifies the minor version of the Automation type library
        /// </summary>
        public uint LibidMinorVersion { get; set; }

        /// <summary>
        /// The LCID of the Automation type library
        /// </summary>
        public object LibidLcid { get; set; } // todo: not object...

        /// <summary>
        /// The path to the Automation type library
        /// </summary>
        public string LibidPath { get; set; }

        /// <summary>
        /// The Automation type library's display name
        /// </summary>
        public string LibidRegName { get; set; }


        public static void Setup(ISyntax Syntax)
        {
           

        }

    }
}
