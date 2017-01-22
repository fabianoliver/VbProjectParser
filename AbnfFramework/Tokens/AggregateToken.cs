using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AbnfFramework.Tokens
{
    public class AggregateToken : Token
    {
        protected readonly Token[] UnderlyingTokens;


        public AggregateToken(Token[] UnderlyingTokens)
        {
            this.UnderlyingTokens = UnderlyingTokens;
        }

        public override string GetAbnfSyntaxRepresentationFor(object obj)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var token in this.UnderlyingTokens)
                sb.Append(token.GetAbnfSyntaxRepresentationFor(obj));

            return sb.ToString();
        }

        public override string GetRegexPattern(MemberInfo Source)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var token in this.UnderlyingTokens)
                sb.Append(token.GetRegexPattern(Source));

            return sb.ToString();
        }
    }
}
