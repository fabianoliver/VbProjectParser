using AbnfFramework.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AbnfFramework.Tokens
{
    public class RegexToken : Token
    {
        private readonly string RegexMatchingPattern;
        private readonly string StringRealization;

        /// <param name="RegexMatchingPattern">A pattern with which to match all possible realizations</param>
        /// <param name="StringRealization">One specific string realization matching RegexMatchingPattern</param>
        public RegexToken(string RegexMatchingPattern, string StringRealization)
        {
            this.RegexMatchingPattern = RegexMatchingPattern;
            this.StringRealization = StringRealization;
        }

        public override string GetAbnfSyntaxRepresentationFor(object obj)
        {
            return StringRealization;
        }

        public override string GetRegexPattern(MemberInfo Source)
        {
            return "(?:" + RegexMatchingPattern + ")";
        }
    }
}
