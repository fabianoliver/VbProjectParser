using AbnfFramework;
using AbnfFramework.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data.ABNF
{
    public class VBAPROJECTText
    {
        public ProjectProperties ProjectProperties { get; set; }

        public HostExtenders HostExtenders { get; set; }

        public ProjectWorkspace ProjectWorkspace { get; set; }

        public static void Setup(ISyntax Syntax)
        {
            Syntax
                .Entity<VBAPROJECTText>()
                .Property(x => x.ProjectProperties)
                .ByRegisteredTypes(typeof(ProjectProperties))
                .WithPostfix(CommonTokens.NWLN);

            Syntax
                .Entity<VBAPROJECTText>()
                .Property(x => x.HostExtenders)
                .ByRegisteredTypes(typeof(HostExtenders))
                .WithPrefix(new LiteralToken("[Host Extender Info]") + CommonTokens.NWLN)
                .WithPostfix(CommonTokens.NWLN);

            Syntax
                .Entity<VBAPROJECTText>()
                .Property(x => x.ProjectWorkspace)
                .ByRegisteredTypes(typeof(ProjectWorkspace))
                .WithPrefix(new LiteralToken("[Workspace]") + CommonTokens.NWLN)
                .IsOptional();
        }
    }
}
