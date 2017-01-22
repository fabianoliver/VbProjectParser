using System.Reflection;
using System.Text.RegularExpressions;

namespace AbnfFramework.Tokens
{
    public class LiteralToken : Token
    {
        public string StringLiteral { get; private set; }

        public LiteralToken(string StringLiteral)
        {
            this.StringLiteral = StringLiteral;
        }

        public override string GetRegexPattern(MemberInfo Source)
        {
            var pattern = Regex.Escape(StringLiteral);
            return pattern;
        }

        public override string GetAbnfSyntaxRepresentationFor(/*MemberInfo source, */object obj)
        {
            return StringLiteral;
        }
    }
}
