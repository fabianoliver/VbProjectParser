using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace AbnfFramework
{
    public interface IEntityBuilder
    {
        Type EntityType { get; }
        Syntax OwningSyntax { get; }
        IValueConverter DefaultConverter { get; }
        string GetRegexPattern();
        string ToAbnfSyntax(object obj);
        IPropertyBuilder GetPropertyBuilderFor(MemberInfo SelectedMember);
        object FromAbnfSyntax(string syntax);
        bool CanParse(string syntax);
    }

    public interface IEntityBuilder<T> : IEntityBuilder
        where T : class
    {
        string ToAbnfSyntax(T obj);
        new T FromAbnfSyntax(string syntax);
        IEntityBuilder<T> HasBaseType<TBase>() where TBase : class;
        IDictionary<MemberInfo, IPropertyBuilder> GetPropertyBuilders();
        IPropertyBuilder<T, TProperty> Property<TProperty>(Expression<Func<T, TProperty>> PropertySelector);
        IPropertyBuilder<T, IEnumerable<TProperty>> EnumerableProperty<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> PropertySelector, Nullable<int> MinCount = null, Nullable<int> MaxCount = null);
    }
}
