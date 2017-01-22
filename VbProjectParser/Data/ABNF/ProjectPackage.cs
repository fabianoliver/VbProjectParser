using AbnfFramework;
using AbnfFramework.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data.ABNF
{
    public class ProjectPackage : ProjectItem
    {
        public Guid GUID { get; set; }

        public static void Setup(ISyntax Syntax)
        {
            Syntax
                .Entity<ProjectPackage>()
                .Property(x => x.GUID)
                .ByRegexPattern(CommonRegexPatterns._GUID)
                .WithPrefix(new LiteralToken("Package="));
        }
    }
}
