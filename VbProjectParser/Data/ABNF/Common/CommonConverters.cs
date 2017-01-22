using AbnfFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VbProjectParser.Data.ABNF
{
    public abstract class ConverterBase<T> : IValueConverter<T>
    {
        public abstract string ConvertToString(T value);
        public abstract T ConvertBack(string text);

        public string ConvertToString(object value)
        {
            if (!(value is T))
                throw new ArgumentException(String.Format("Expected value {0} to be of type {1}", value, typeof(T).Name), "value");

            return ConvertToString((T)value);
        }

        public object ConvertBack(string text, Type TargetType)
        {
            if (TargetType != typeof(T))
                throw new ArgumentException(String.Format("Expected TargetType to be {0}, but was {1}", typeof(T).Name, TargetType.Name), "TargetType");

            return ConvertBack(text);
        }
    }

    public class Int32Converter : ConverterBase<int>
    {
        public override string ConvertToString(int value)
        {
            string result = value.ToString();
            return result;
        }

        public override int ConvertBack(string text)
        {
            if (!Regex.IsMatch(text, CommonRegexPatterns._INT32))
                throw new ArgumentException(String.Format("Argument ({0}) was not in valid format", text), "text");

            var result = Convert.ToInt32(text);
            return result;
        }
    }

    public class HexInt32Converter : ConverterBase<int>
    {
        public override string ConvertToString(int value)
        {  
            string numberPart = value.ToString("X8");
            string result = @"&H" + numberPart;
            return result;
        }

        public override int ConvertBack(string text)
        {
            if (!Regex.IsMatch(text, CommonRegexPatterns._HEXINT32))
                throw new ArgumentException(String.Format("Argument ({0}) was not in valid format", text), "text");

            var regex = new Regex(@"\&H(?<NumberPart>[\x30-\x39ABCDEF]{8})");
            var match = regex.Match(text);
            string numPart = match.Groups["NumberPart"].Value;
            var result = Convert.ToInt32(numPart, 16);
            return result;
        }
    }

    public class GuidConverter : ConverterBase<Guid>
    {
        public override string ConvertToString(Guid value)
        {
            var result = "{" + value.ToString().ToUpper() + "}";
            return result;
        }

        public override Guid ConvertBack(string text)
        {
            if (!Regex.IsMatch(text, CommonRegexPatterns._GUID))
                throw new ArgumentException(String.Format("Argument ({0}) was not in valid format", text), "text");

            var result = new Guid(text);
            return result;
        }
    }

    public static class CommonConverters
    {
        public static readonly HexInt32Converter HexInt32Converter = new HexInt32Converter();
        public static readonly GuidConverter GuidConverter = new GuidConverter();
        public static readonly Int32Converter Int32Converter = new Int32Converter();
    }
}
