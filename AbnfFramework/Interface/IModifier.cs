using System;
using System.Reflection;
using System.Text;

namespace AbnfFramework
{
    public interface IModifier
    {
        int ProcessingPriority { get; }

        string DecorateRegexPattern(IEntityBuilder caller, MemberInfo Source, string InitialPattern);

        object ModifyTargetValue(IEntityBuilder caller, Type SourcePropertyType, object NewValue);

        //void ModifyAbnfSyntaxRepresentationFor(IAbnfEntitySyntaxBuilder caller, MemberInfo source, object obj, StringBuilder representation);
        void ModifyAbnfSyntaxRepresentationFor(IEntityBuilder caller, ref object value, StringBuilder representation);
    }
}
