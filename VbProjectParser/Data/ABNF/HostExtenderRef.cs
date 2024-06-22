using AbnfFramework;
using AbnfFramework.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data.ABNF
{
    // p 26
    /// <summary>
    /// Specifies a reference to an aggregatable server’s Automation type library
    /// </summary>
    public class HostExtenderRef
    {
        /// <summary>
        /// Specifies the index of the host extender entry. MUST be unique to the list of HostExtenders
        /// </summary>
        public int ExtenderIndex { get; set; }

        /// <summary>
        /// Specifies the GUID of the Automation type library to extend.
        /// </summary>
        public Guid ExtenderGuid { get; set; }

        /// <summary>
        /// Specifies a host-provided Automation type library name. "VBE" specifies a built in name for the VBA Automation type library.
        /// </summary>
        public string LibName { get; set; }

        /// <summary>
        /// Specifies a host-provided flag as follows:
        /// 0x00000000 = MUST NOT create a new extended type library for the aggregatable server if one is already available to the VBA environment.
        //  0x00000001 = MUST create a new extended type library for the aggregatable server.
        /// </summary>
        public int CreationFlags { get; set; }

        public static void Setup(ISyntax Syntax)
        {
            Syntax
                .Entity<HostExtenderRef>()
                .Property(x => x.ExtenderIndex)
                .ByRegexPattern(CommonRegexPatterns._HEXINT32, CommonConverters.HexInt32Converter)
                .WithPostfix(new LiteralToken("="));

            Syntax
                .Entity<HostExtenderRef>()
                .Property(x => x.ExtenderGuid)
                .ByRegexPattern(CommonRegexPatterns._GUID)
                .WithPostfix(new LiteralToken(";"));

            Syntax
                .Entity<HostExtenderRef>()
                .Property(x => x.LibName)
                .ByRegexPattern("VBE|[\x21-\x3A\x3C-\xFF]*")
                .WithPostfix(new LiteralToken(";"));

            Syntax
                .Entity<HostExtenderRef>()
                .Property(x => x.CreationFlags)
                .ByRegexPattern(CommonRegexPatterns._HEXINT32, CommonConverters.HexInt32Converter)
                .WithPostfix(CommonTokens.NWLN);
        }
    }
}
