using System.Reflection;

namespace AbnfFramework.Tokens
{
    public abstract class Token
    {
        public abstract string GetRegexPattern(MemberInfo Source);
        public abstract string GetAbnfSyntaxRepresentationFor(object obj);

        public static Token operator +(Token token1, Token token2)
        {
            return new AggregateToken(new Token[] { token1, token2 });
        }
    }
}
