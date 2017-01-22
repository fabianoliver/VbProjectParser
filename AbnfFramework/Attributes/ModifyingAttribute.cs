using System;
using System.Reflection;
using System.Text;

namespace AbnfFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ModifyingAttribute : Attribute, IModifier
    {
        int IModifier.ProcessingPriority { get { throw new InvalidOperationException(); } }

        public virtual string DecorateRegexPattern(IEntityBuilder caller, MemberInfo Source, string InitialPattern)
        {
            return InitialPattern;
        }

        public virtual object ModifyTargetValue(IEntityBuilder caller, Type SourcePropertyType, object NewValue)
        {
            return NewValue;
        }

        public virtual void ModifyAbnfSyntaxRepresentationFor(IEntityBuilder caller, /*MemberInfo source, */ref object obj, StringBuilder representation)
        {
            return;
        }

    }
}
