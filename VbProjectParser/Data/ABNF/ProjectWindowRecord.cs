using AbnfFramework;
using AbnfFramework.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data.ABNF
{
    // p 27, 2.3.1.20
    public class ProjectWindowRecord
    {
        /// <summary>
        /// Specifies the name of a module. SHOULD be an identifier as specified by [MS-VBAL] section 3.3.5. MAY<2> be any string of characters. MUST be less than or equal to 31 characters long.
        /// </summary>
        public string ModuleIdentifier { get; set; }

        public ProjectWindowState ProjectWindowState { get; set; }

        public static void Setup(ISyntax Syntax)
        {
            Syntax
                .Entity<ProjectWindowRecord>()
                .Property(x => x.ModuleIdentifier)
                .ByRegexPattern(CommonRegexPatterns._ANYCHAR + "{0,31}")
                .WithPostfix(new LiteralToken("="));

            Syntax
                .Entity<ProjectWindowRecord>()
                .Property(x => x.ProjectWindowState)
                .ByRegisteredTypes(typeof(ProjectWindowState))
                .WithPostfix(CommonTokens.NWLN);
        }
    }
}
