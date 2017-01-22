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

namespace AbnfFramework
{
    public class PropertyBuilder<TObj, TProperty> : IPropertyBuilder<TObj, TProperty>
        where TObj : class
    {
        protected readonly List<IModifier> Modifiers = new List<IModifier>();

        public MemberInfo TargetMember { get; private set; }
        public EntityBuilder<TObj> Builder { get; private set; }

        public override string ToString()
        {
            return String.Format("{0} ({1})", typeof(PropertyBuilder<TObj, TProperty>).Name, this.TargetMember);
        }

        public PropertyBuilder(EntityBuilder<TObj> Builder, MemberInfo TargetMember)
        {
            this.Builder = Builder;
            this.TargetMember = TargetMember;

            ReadModifierAttributes();
        }

        void IPropertyBuilder.ModifyObj(object obj, string FromText)
        {
            ModifyObj(obj as TObj, FromText);
        }

        public virtual void ModifyObj(TObj obj, string FromText)
        {
            if (String.IsNullOrWhiteSpace(FromText))
                return;

            object property_value = FromText;

            var modifiers = GetModifiers().Reverse();
            foreach (var modifier in modifiers)
            {
                property_value = modifier.ModifyTargetValue(this.Builder, this.TargetMember.GetUnderlyingType(), property_value);
            }

            TargetMember.SetValue(obj, property_value);
        }

        public virtual string ToAbnfSyntax(object obj)
        {
            var result = new StringBuilder();

            object value = this.TargetMember.GetValue(obj);

            foreach (var modifier in GetModifiers())
            {
                modifier.ModifyAbnfSyntaxRepresentationFor(this.Builder, /*TargetMember, */ref value, result);
            }

            return result.ToString();
        }

        public IOrderedEnumerable<IModifier> GetModifiers()
        {
            return Modifiers.OrderBy(x => x.ProcessingPriority);
        }

        public virtual string GetRegex()
        {
            var pattern = String.Empty;

            Trace.WriteLine(String.Format("PropertyBuilder.GetRegex(): Processing {0}.{1} - begin", this.TargetMember.DeclaringType.Name, this.TargetMember.Name));

            var modifiers = GetModifiers();
            foreach (var modifier in modifiers)
            {
                Trace.WriteLine(String.Format("PropertyBuilder.GetRegex(): Processing {0}.{1} - applying modifier {2}", this.TargetMember.DeclaringType.Name, this.TargetMember.Name, modifier));
                pattern = modifier.DecorateRegexPattern(this.Builder, this.TargetMember, pattern);
            }

            Trace.WriteLine(String.Format("PropertyBuilder.GetRegex(): Processing {0}.{1} - completed", this.TargetMember.DeclaringType.Name, this.TargetMember.Name));

            return pattern;
        }

        private void ReadModifierAttributes()
        {
            var attributes = this.TargetMember.GetCustomAttributes<ModifyingAttribute>(true);
            this.Modifiers.AddRange(attributes);
        }

        public IPropertyBuilder<TObj, TProperty> ByRegexPattern(string RegexPattern)
        {
            var modifier = new ValueFromRegexAttribute(RegexPattern);
            Modifiers.Add(modifier);
            return this;
        }

        public IPropertyBuilder<TObj, TProperty> ByRegexPattern(string RegexPattern, IValueConverter Converter)
        {
            var modifier = new ValueFromRegexAttribute(RegexPattern, Converter);
            Modifiers.Add(modifier);
            return this;
        }

        public IPropertyBuilder<TObj, TProperty> ByRegisteredTypes(params Type[] types)
        {
            var modifier = new ValueFromRegisteredTypesAttribute(types);
            Modifiers.Add(modifier);
            return this;
        }

        public IPropertyBuilder<TObj, TProperty> WithPrefix(Token Prefix)
        {
            var modifier = new TokenModifier(AbnfFramework.Tokens.TokenModifier.AppendDirection.Prefix, Prefix);
            Modifiers.Add(modifier);
            return this;
        }

        public IPropertyBuilder<TObj, TProperty> WithPostfix(Token Postfix)
        {
            var modifier = new TokenModifier(AbnfFramework.Tokens.TokenModifier.AppendDirection.Postfix, Postfix);
            Modifiers.Add(modifier);
            return this;
        }

        public IPropertyBuilder<TObj, TProperty> IsOptional()
        {
            var modifier = new IsOptionalAttribute();
            Modifiers.Add(modifier);
            return this;
        }
    }
}
