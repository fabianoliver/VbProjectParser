using AbnfFramework.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data.ABNF
{
   
    public class CommonRegexPatterns
    {
        public const string _CR = @"[\x0D]"; // carriage return
        public const string _DIGIT = @"[\x30-\x39]";
        public const string _HEXDIG = @"[\x30-\x39ABCDEF]";
        public const string _DQUOTE = @"[\x22]";
        public const string _HTAB = @"[\x09]";
        public const string _LF = @"[\x0A]";
        public const string _SP = @"[\x20]";
        public const string _VCHAR = @"[\x21-\x7E]";
        public const string _WSP = @"[\x20\x09]";
        public const string _ANYCHAR = @"[\x01-\x09\x0B\x0C\x0E-\xFF]";


        public const string _EQ = @"(?:[\x20]|[\x09])*=(?:[\x20]|[\x09])*";

        //public const string _SIGN = @"[+-]";
        //public const string _EXP = @"e[+-]?[0-9]+";
        public const string _FLOAT = @"[+-]?([0-9]+\.[0-9]+(?:e[+-]?[0-9]+)?|\.[0-9]+(?:e[+-]?[0-9]+)?|[0-9]+\.?(?:e[+-]?[0-9]+)?)";
        public const string _GUID = @"\{[\x30-\x39ABCDEF]{8}-[\x30-\x39ABCDEF]{4}-[\x30-\x39ABCDEF]{4}-[\x30-\x39ABCDEF]{4}-[\x30-\x39ABCDEF]{12}\}";

        public const string _HEXINT32 = @"\&H[\x30-\x39ABCDEF]{8}";
        public const string _INT32 = @"[-]?[0-9]+";

        /// <summary>
        /// Specifies a newline
        /// </summary>
        public const string _NWLN = @"\r\n|\n\r";
        public const string _NQCHAR = @"[\x21\x23-\xFF]";
        public const string _QUOTEDCHAR = _WSP + "|" + _NQCHAR + "|" + _DQUOTE + @"{2}";

        /// <summary>
        /// An array of characters that specifies a path to a file. MUST be less than 260 characters.
        /// </summary>
        public const string _PATH = _DQUOTE + "(" + _QUOTEDCHAR + "){0,259}" + _DQUOTE;

        public const string _VBABOOL = "[0 -1]";

    }
}
