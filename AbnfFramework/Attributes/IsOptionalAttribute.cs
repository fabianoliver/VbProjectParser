using System;
using System.Reflection;
using System.Text;

namespace AbnfFramework.Attributes
{
    public class IsOptionalAttribute : ModifyingAttribute, IModifier
    {
        public static readonly int ProcessingPriority = ValueFromRegexAttribute.ProcessingPriority + 50;
        int IModifier.ProcessingPriority { get { return ProcessingPriority; } }


        public IsOptionalAttribute()
            : base()
        {
        }

        public override string DecorateRegexPattern(IEntityBuilder caller, MemberInfo Source, string InitialPattern)
        {
            if (String.IsNullOrEmpty(InitialPattern))
                throw new ArgumentException("Expected InitialPattern not to be null or empty", "InitialPattern");

            return "(" + InitialPattern + ")?";
        }

        public override object ModifyTargetValue(IEntityBuilder caller, Type SourcePropertyType, object NewValue)
        {
            return NewValue;
        }

        public override void ModifyAbnfSyntaxRepresentationFor(IEntityBuilder caller, ref object value, StringBuilder representation)
        {
            if (value == null)
            {
                representation.Clear();
            }
        }
    }
}
