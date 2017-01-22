using AbnfFramework;
using AbnfFramework.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data.ABNF
{
    public abstract class ProjectModule : ProjectItem
    {
    }

    public class ProjectDocModule : ProjectModule
    {
        /// <summary>
        /// Specifies the name of a module. SHOULD be an identifier as specified by [MS-VBAL] section 3.3.5. MAY<2> be any string of characters. MUST be less than or equal to 31 characters long.
        /// </summary>
        public string ModuleIdentifier { get; set; }

        /// <summary>
        /// Specifies the document module’s Automation server version as specified by [MS-OAUT].
        /// </summary>
        public int DocTlibVer { get; set; }

        public static void Setup(ISyntax Syntax)
        {
            Syntax
                .Entity<ProjectDocModule>()
                .Property(x => x.ModuleIdentifier)
                .ByRegexPattern(CommonRegexPatterns._ANYCHAR + "{0,31}")
                .WithPrefix(new LiteralToken("Document="));

            Syntax
                .Entity<ProjectDocModule>()
                .Property(x => x.DocTlibVer)
                .ByRegexPattern(CommonRegexPatterns._HEXINT32, CommonConverters.HexInt32Converter)
                .WithPrefix(new LiteralToken(@"/"));
        }
    }

    public class ProjectStdModule : ProjectModule
    {
        /// <summary>
        /// Specifies the name of a module. SHOULD be an identifier as specified by [MS-VBAL] section 3.3.5. MAY<2> be any string of characters. MUST be less than or equal to 31 characters long.
        /// </summary>
        public string ModuleIdentifier { get; set; }

        public static void Setup(ISyntax Syntax)
        {
            Syntax
               .Entity<ProjectStdModule>()
               .Property(x => x.ModuleIdentifier)
               .ByRegexPattern(CommonRegexPatterns._ANYCHAR + "{0,31}")
               .WithPrefix(new LiteralToken("Module="));
        }
    }

    public class ProjectClassModule : ProjectModule
    {
        /// <summary>
        /// Specifies the name of a module. SHOULD be an identifier as specified by [MS-VBAL] section 3.3.5. MAY<2> be any string of characters. MUST be less than or equal to 31 characters long.
        /// </summary>
        public string ModuleIdentifier { get; set; }

        public static void Setup(ISyntax Syntax)
        {
            Syntax
               .Entity<ProjectClassModule>()
               .Property(x => x.ModuleIdentifier)
               .ByRegexPattern(CommonRegexPatterns._ANYCHAR + "{0,31}")
               .WithPrefix(new LiteralToken("Class="));
        }
    }

    public class ProjectDesignerModule : ProjectModule
    {
        /// <summary>
        /// Specifies the name of a module. SHOULD be an identifier as specified by [MS-VBAL] section 3.3.5. MAY<2> be any string of characters. MUST be less than or equal to 31 characters long.
        /// </summary>
        public string ModuleIdentifier { get; set; }

        public static void Setup(ISyntax Syntax)
        {
            Syntax
              .Entity<ProjectDesignerModule>()
              .Property(x => x.ModuleIdentifier)
              .ByRegexPattern(CommonRegexPatterns._ANYCHAR + "{0,31}")
              .WithPrefix(new LiteralToken("BaseClass="));
        }
    }



}
