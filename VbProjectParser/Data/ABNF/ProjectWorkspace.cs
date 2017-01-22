using AbnfFramework;
using AbnfFramework.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data.ABNF
{
    public class ProjectWorkspace
    {
        public IList<ProjectWindowRecord> ProjectWindowRecords { get; set; }

        public ProjectWorkspace()
        {
            this.ProjectWindowRecords = new List<ProjectWindowRecord>();
        }

        public static void Setup(ISyntax Syntax)
        {
            Syntax
                .Entity<ProjectWorkspace>()
                .EnumerableProperty(x => x.ProjectWindowRecords)
                .ByRegisteredTypes(typeof(ProjectWindowRecord));          
        }
    }
}
