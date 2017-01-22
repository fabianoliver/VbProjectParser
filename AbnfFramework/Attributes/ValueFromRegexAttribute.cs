using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AbnfFramework.Attributes
{
    public class ValueFromRegexAttribute : ValueModifyingAttribute, IModifier
    {
        public static readonly int ProcessingPriority = 100000;
        int IModifier.ProcessingPriority { get { return ProcessingPriority; } }

        public string RegexPattern { get; set; }

        public ValueFromRegexAttribute(string RegexPattern, Type ValueConverterType)
            : base(ValueConverterType)
        {
            this.RegexPattern = RegexPattern;
        }

        public ValueFromRegexAttribute(string RegexPattern, IValueConverter ConverterInstance)
            : base(ConverterInstance)
        {
            this.RegexPattern = RegexPattern;
        }

        public ValueFromRegexAttribute(string RegexPattern)
            : base()
        {
            this.RegexPattern = RegexPattern;
        }

        public override string DecorateRegexPattern(IEntityBuilder caller, MemberInfo Source, string InitialPattern)
        {
            if (!String.IsNullOrEmpty(InitialPattern))
                throw new ArgumentException("Expected InitialPattern to be null or empty", "InitialPattern");

            return this.RegexPattern;
        }

        public override object ModifyTargetValue(IEntityBuilder caller, Type SourcePropertyType, object NewValue)
        {
            if (SourcePropertyType == null)
                throw new ArgumentNullException("SourcePropertyType");

            if (!(NewValue is string))
                throw new ArgumentException(String.Format("Expected parameter ({0}) to be of type string", NewValue), "NewValue");

            string FromText = (string)NewValue;
            var regex = new Regex(RegexPattern);
            var match = regex.Match(FromText);

            if (!match.Success)
                throw new InvalidOperationException(String.Format("Could not match text {0} with pattern {1}", FromText, RegexPattern));

            string strResult = match.Value;

            Type TargetType = SourcePropertyType;

            if (TargetType == typeof(string))
                return strResult;

            var converter = GetConverterFor(TargetType);

            if (converter == null)
                converter = caller.DefaultConverter;

            if (converter == null)
                throw new NullReferenceException(String.Format("Could not find/create an approrpiate converter for type {0}", TargetType.Name));

            var convertedResult = converter.ConvertBack(strResult, TargetType);
            return convertedResult;
        }

        public override void ModifyAbnfSyntaxRepresentationFor(IEntityBuilder caller, ref object value, StringBuilder representation)
        {
            if (value == null)
                return;

            string strValue;

            if (value is string)
            {
                strValue = (string)value;
            }
            else
            {
                Type TargetType = value.GetType();
                var converter = GetConverterFor(TargetType);

                if (converter == null)
                    converter = caller.DefaultConverter;

                if (converter == null)
                    throw new NullReferenceException(String.Format("Could not find/create an approrpiate converter for type {0}", TargetType.Name));

                strValue = converter.ConvertToString(value);
            }

            representation.Append(strValue);
        }
    }
}
