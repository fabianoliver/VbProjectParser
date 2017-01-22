using AbnfFramework.Tokens;
using System;
using System.Linq;
using System.Reflection;

namespace AbnfFramework
{
    public interface IPropertyBuilder
    {
        MemberInfo TargetMember { get; }
        IOrderedEnumerable<IModifier> GetModifiers();
        string GetRegex();
        string ToAbnfSyntax(object obj);
        void ModifyObj(object obj, string FromText);
    }

    public interface IPropertyBuilder<TObj, out TProperty> : IPropertyBuilder
        where TObj : class
    {
        EntityBuilder<TObj> Builder { get; }

        IPropertyBuilder<TObj, TProperty> ByRegexPattern(string RegexPattern);
        IPropertyBuilder<TObj, TProperty> ByRegexPattern(string RegexPattern, IValueConverter Converter);
        IPropertyBuilder<TObj, TProperty> ByRegisteredTypes(params Type[] types);
        IPropertyBuilder<TObj, TProperty> WithPrefix(Token Prefix);
        IPropertyBuilder<TObj, TProperty> WithPostfix(Token Postfix);
        IPropertyBuilder<TObj, TProperty> IsOptional();

        void ModifyObj(TObj obj, string FromText);
    }
}
