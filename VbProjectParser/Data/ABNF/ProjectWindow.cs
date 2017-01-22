using AbnfFramework;
using AbnfFramework.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Data.ABNF;

namespace VbProjectParser.Data
{
    public class ProjectWindow
    {
        public int WindowLeft { get; set; }
        public int WindowTop { get; set; }
        public int WindowRight { get; set; }
        public int WindowBottom { get; set; }

        /// <summary>
        /// "C", "Z", or "I".
        /// C = Closed.
        /// Z = Zoomed to fill the available viewing area.
        /// I = minimized to an icon
        /// </summary>
        public string WindowState { get; set; }

        public static void Setup(ISyntax Syntax)
        {
            Syntax
                .Entity<ProjectWindow>()
                .Property(x => x.WindowLeft)
                .ByRegexPattern(CommonRegexPatterns._INT32)
                .WithPostfix(new LiteralToken(", "));

            Syntax
                .Entity<ProjectWindow>()
                .Property(x => x.WindowTop)
                .ByRegexPattern(CommonRegexPatterns._INT32)
                .WithPostfix(new LiteralToken(", "));

            Syntax
                .Entity<ProjectWindow>()
                .Property(x => x.WindowRight)
                .ByRegexPattern(CommonRegexPatterns._INT32)
                .WithPostfix(new LiteralToken(", "));

            Syntax
                .Entity<ProjectWindow>()
                .Property(x => x.WindowBottom)
                .ByRegexPattern(CommonRegexPatterns._INT32)
                .WithPostfix(new LiteralToken(", "));

            Syntax
                .Entity<ProjectWindow>()
                .Property(x => x.WindowState)
                .ByRegexPattern("[CZI]{0,1}"); // todo: documentation not quite clear to me.. is could there be two chars? e.g. CZ ?
        }
    }
}
