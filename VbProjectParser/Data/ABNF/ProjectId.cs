using AbnfFramework;
using AbnfFramework.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data.ABNF
{
    // p. 21
    public class ProjectId 
    {
        public Guid ProjectCLSID;

        public static void Setup(ISyntax Syntax)
        {
            Syntax
                .Entity<ProjectId>()
                .Property(x => x.ProjectCLSID)
                .ByRegexPattern(CommonRegexPatterns._GUID)
                .WithPrefix(new LiteralToken("ID=\""))
                .WithPostfix(CommonTokens.DQUOTE + CommonTokens.NWLN);
        }
     
    }

}
