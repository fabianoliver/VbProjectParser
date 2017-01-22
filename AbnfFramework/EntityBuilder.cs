using AbnfFramework.Attributes;
using AbnfFramework.Converters;
using AbnfFramework.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AbnfFramework.Extensions;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.ObjectModel;

namespace AbnfFramework
{
    public class EntityBuilder<TObj> : IEntityBuilder<TObj>
        where TObj : class
    {
        private IDictionary<MemberInfo, IPropertyBuilder> _PropertyBuilders = new Dictionary<MemberInfo, IPropertyBuilder>();
        public IValueConverter DefaultConverter { get; private set; }
        private readonly Syntax ParentSyntax;

        public Syntax OwningSyntax
        {
            get
            {
                return ParentSyntax;
            }
        }

        public Type EntityType
        {
            get
            {
                return typeof(TObj);
            }
        }

        public EntityBuilder(Syntax ParentSyntax, IValueConverter DefaultConverter)
        {
            this.ParentSyntax = ParentSyntax;
            this.DefaultConverter = DefaultConverter;
        }

        public IPropertyBuilder GetPropertyBuilderFor(MemberInfo SelectedMember)
        {
            return _PropertyBuilders[SelectedMember];
        }

        public IDictionary<MemberInfo, IPropertyBuilder> GetPropertyBuilders()
        {
            return new ReadOnlyDictionary<MemberInfo, IPropertyBuilder>(_PropertyBuilders);
        }

        public IPropertyBuilder<TObj, IEnumerable<TProperty>> EnumerableProperty<TProperty>(Expression<Func<TObj, IEnumerable<TProperty>>> PropertySelector, Nullable<int> MinCount = null, Nullable<int> MaxCount = null)
        {
            if (PropertySelector == null)
                throw new ArgumentNullException("PropertySelector");

            MemberInfo mi = GetMemberInfo(PropertySelector);

            if (mi == null)
                throw new ArgumentException(String.Format("Could not get MemberInfo from {0}. Make sure the expression is correct.", PropertySelector), "PropertySelector");

            if (!_PropertyBuilders.ContainsKey(mi))
            {
                var builder = new EnumerablePropertyBuilder<TObj, TProperty>(this, mi, typeof(TProperty), MinCount, MaxCount);
                _PropertyBuilders.Add(mi, builder);
            }

            return (IPropertyBuilder<TObj, IEnumerable<TProperty>>)_PropertyBuilders[mi];
        }

        public IPropertyBuilder<TObj, TProperty> Property<TProperty>(Expression<Func<TObj, TProperty>> PropertySelector)
        {
            if (PropertySelector == null)
                throw new ArgumentNullException("PropertySelector");

            MemberInfo mi = GetMemberInfo(PropertySelector);

            if (mi == null)
                throw new ArgumentException(String.Format("Could not get MemberInfo from {0}. Make sure the expression is correct.", PropertySelector), "PropertySelector");

            if (!_PropertyBuilders.ContainsKey(mi))
            {
                var builder = new PropertyBuilder<TObj, TProperty>(this, mi);
                _PropertyBuilders.Add(mi, builder);
            }

            return (IPropertyBuilder<TObj, TProperty>)_PropertyBuilders[mi];
        }

        public IEntityBuilder<TObj> HasBaseType<TBaseType>()
            where TBaseType : class
        {
            if (ParentSyntax.Entity(typeof(TBaseType)) == null)
                throw new ArgumentException(String.Format("No builder for type {0} registered. Make sure to call .HasBaseType() on an inheriting type AFTER having set up the parent type within the builder", typeof(TBaseType)), "TBaseType");

            if (!typeof(TBaseType).IsAssignableFrom(typeof(TObj)))
                throw new ArgumentException(String.Format("{0} does not inherit from {1}", typeof(TObj).FullName, typeof(TBaseType).FullName), "TBaseType");

            var ParentBuilder = ParentSyntax.Entity<TBaseType>();
            foreach (var kvp in ParentBuilder.GetPropertyBuilders())
            {
                this._PropertyBuilders.Add(kvp);
            }

            return this;
        }

        public string GetRegexPattern()
        {
            var result = new StringBuilder();

            foreach (var kvp in _PropertyBuilders)
            {
                var builder = kvp.Value;
                Trace.WriteLine(String.Format("Processing builder {0}", builder));

                string capture_group_name = GetRegexGroupNameFor(kvp.Key, kvp.Value);

                string pattern = @"(?<" + capture_group_name + ">";
                pattern += builder.GetRegex();
                pattern += "(?# end " + capture_group_name +")";
                pattern += ")";
                result.Append(pattern);
            }

            return result.ToString();
        }

        private string GetRegexGroupNameFor(MemberInfo mi, IPropertyBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");

            return String.Format("Grp_{0}", mi.Name);
        }

        public bool CanParse(string syntax)
        {
            if (String.IsNullOrWhiteSpace(syntax))
                return true; // debug.. hmm todo

            var regexPattern = "^" + GetRegexPattern() + "$";
            var regex = new Regex(regexPattern);
            var result = regex.IsMatch(syntax);
            return result;
        }

        object IEntityBuilder.FromAbnfSyntax(string syntax)
        {
            return FromAbnfSyntax(syntax);
        }


        public TObj FromAbnfSyntax(string syntax)
        {
            if (String.IsNullOrWhiteSpace(syntax) && !this.EntityType.IsValueType)
                return default(TObj); // todo: hmm..


            TObj result = (TObj)Activator.CreateInstance(typeof(TObj));

            var regexPattern = "^" + GetRegexPattern() + "$";
            var regex = new Regex(regexPattern);
            var match = regex.Match(syntax);

            if (!match.Success)
                throw new InvalidOperationException(String.Format("For entity type {0}, could not match syntax {1} with regex pattern {2}", this.EntityType.Name, syntax, regexPattern));

            foreach (var kvp in _PropertyBuilders)
            {
                var builder = kvp.Value;
                string capture_group_name = GetRegexGroupNameFor(kvp.Key, kvp.Value);
                var captured_group = match.Groups[capture_group_name];

                if (captured_group == null)
                    throw new InvalidOperationException(String.Format("Did not find any results for capture group {0}", capture_group_name));

                string captured_value = captured_group.Value;
                builder.ModifyObj(result, captured_value);
            }

            return result;
        }

        public string ToAbnfSyntax(TObj obj)
        {
            var result = new StringBuilder();

            foreach (var builder in _PropertyBuilders.Values)
            {
                var abnf = builder.ToAbnfSyntax(obj);
                result.Append(abnf);
            }

            return result.ToString();
        }

        string IEntityBuilder.ToAbnfSyntax(object obj)
        {
            return ToAbnfSyntax(obj as TObj);
        }

        private static MemberInfo GetMemberInfo(LambdaExpression lambda)
        {
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
                memberExpression = (MemberExpression)lambda.Body;

            return memberExpression.Member;
        }
    }
}
