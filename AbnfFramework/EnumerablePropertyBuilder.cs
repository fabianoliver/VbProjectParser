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
    public class EnumerablePropertyBuilder<TObj, TProperty> : PropertyBuilder<TObj, IEnumerable<TProperty>>
        where TObj : class
    {
        protected readonly Nullable<int> MinCount;
        protected readonly Nullable<int> MaxCount;

        protected Func<IList> ListFactory;
        protected Type UnderlyingElementType;

        public EnumerablePropertyBuilder(EntityBuilder<TObj> Builder, MemberInfo TargetMember, Type UnderlyingElementType, Func<IList> ListFactory, Nullable<int> MinCount, Nullable<int> MaxCount)
            : base(Builder, TargetMember)
        {
            if (ListFactory == null)
                ListFactory = () => CreateList();

            this.ListFactory = ListFactory;

            this.MinCount = MinCount;
            this.MaxCount = MaxCount;
            this.UnderlyingElementType = UnderlyingElementType;
        }

        public EnumerablePropertyBuilder(EntityBuilder<TObj> Builder, MemberInfo TargetMember, Type UnderlyingElementType, Nullable<int> MinCount, Nullable<int> MaxCount)
            : this(Builder, TargetMember, UnderlyingElementType, null, MinCount, MaxCount)
        {
        }

        public override void ModifyObj(TObj obj, string FromText)
        {
            var regexPattern = GetRegex();
            var regex = new Regex(regexPattern);

            var match = regex.Match(FromText);

            if (!match.Success)
                throw new InvalidOperationException(String.Format("Could not match regex {0} to input {1}", regexPattern, FromText));

            string capture_name = GetEnumerableGroupCaptureName();
            var captures = match.Groups[capture_name].Captures.Cast<Capture>();

            IList list = ListFactory();

            Trace.WriteLine(String.Format("EnumerablePropertyBuilder.GetRegex(): Processing {0}.{1} - begin", this.TargetMember.DeclaringType.Name, this.TargetMember.Name));

            foreach (var capture in captures)
            {
                object property_value = capture.Value;
                Trace.WriteLine(String.Format("EnumerablePropertyBuilder.GetRegex(): Processing {0}.{1} - enumerable match {2} - begin", this.TargetMember.DeclaringType.Name, this.TargetMember.Name, capture.Value));

                var modifiers = GetModifiers().Reverse().ToArray();
                foreach (var modifier in modifiers)
                {
                    Trace.WriteLine(String.Format("EnumerablePropertyBuilder.GetRegex(): Processing {0}.{1} - enumerable match {2} ({3}) - applying modifier {4}", this.TargetMember.DeclaringType.Name, this.TargetMember.Name, capture.Value, property_value, modifier));
                    property_value = modifier.ModifyTargetValue(this.Builder, this.UnderlyingElementType, property_value);
                }

                Trace.WriteLine(String.Format("EnumerablePropertyBuilder.GetRegex(): Processing {0}.{1} - enumerable match {2} - completed, result {3}", this.TargetMember.DeclaringType.Name, this.TargetMember.Name, capture.Value, property_value));

                list.Add(property_value);
            }

            if (MinCount != null && list.Count < (int)MinCount)
                throw new InvalidOperationException(String.Format("Expected at least {0} items, but found only {1}", (int)MinCount, list.Count));

            if (MaxCount != null && list.Count > (int)MaxCount)
                throw new InvalidOperationException(String.Format("Expected at most {0} items, but found {1}", (int)MaxCount, list.Count));

            Trace.WriteLine(String.Format("EnumerablePropertyBuilder.GetRegex(): Processing {0}.{1} - completed", this.TargetMember.DeclaringType.Name, this.TargetMember.Name));


            TargetMember.SetValue(obj, list);
        }

        private IList CreateList()
        {
            var TargetType = GetListType();
            return (IList)Activator.CreateInstance(TargetType);
        }

        private Type GetListType()
        {
            Type TargetType = this.TargetMember.GetUnderlyingType();
            bool CanCreate = TargetType.GetConstructor(Type.EmptyTypes) != null && !TargetType.IsAbstract;

            // If the member's type is already an IList-based type that is actually creatible (by the Activator),
            // we'll just return that type directly...
            if (typeof(IList).IsAssignableFrom(TargetType) && CanCreate)
                return TargetType;

            // ... otherwise, well check if we can go with List<UnderlyingElementType> instead
            var BoxedType = typeof(List<>).MakeGenericType(this.UnderlyingElementType);

            if (TargetType.IsAssignableFrom(BoxedType))
                return BoxedType;

            throw new InvalidOperationException(String.Format("Cannot create any IList-based type for {0}", TargetType.FullName));
        }

        public override string ToAbnfSyntax(object obj)
        {
            StringBuilder result = new StringBuilder();
            IEnumerable<TProperty> values = (IEnumerable<TProperty>)base.TargetMember.GetValue(obj);

            foreach (TProperty value in values)
            {
                var temp_result = new StringBuilder();
                object val = value;

                var modifiers = GetModifiers();
                foreach (var modifier in modifiers)
                {
                    modifier.ModifyAbnfSyntaxRepresentationFor(this.Builder, /*TargetMember, */ref val, temp_result);
                }

                result.Append(temp_result.ToString());

            }

            return result.ToString();
        }

        public override string GetRegex()
        {
            var singleItemPattern = base.GetRegex();
            StringBuilder result = new StringBuilder();

            string capture_name = GetEnumerableGroupCaptureName();

            result.Append(@"(?<" + capture_name + ">");
            result.Append(singleItemPattern);
            result.Append(@")");

            string quantifier;


            if (HasNoMinCount() && HasNoMaxCount())
            {
                // has no min count and no max count (0 to infinity)
                quantifier = "*";
            }
            else if (HasNoMaxCount() && !HasNoMinCount())
            {
                // Has a minimum count, but no max count (min to infinity)
                int minCount = (int)MinCount;

                quantifier = "{" + minCount + ",}";
            }
            else if (!HasNoMaxCount() && HasNoMinCount())
            {
                // has a max count, but no min count (0 to maxcount)
                int maxCount = (int)MaxCount;
                quantifier = "{0," + maxCount + "}";
            }
            else
            {
                // has a min count and a max count
                int minCount = (int)MinCount;
                int maxCount = (int)MaxCount;
                quantifier = "{" + minCount + "," + maxCount + "}";
            }

            result.Append(quantifier);

            return result.ToString();

        }

        private string GetEnumerableGroupCaptureName()
        {
            return String.Format("EnumerableProperty_{0}", TargetMember.Name);
        }

        private bool HasNoMinCount()
        {
            return this.MinCount == null || this.MinCount < 1;
        }

        private bool HasNoMaxCount()
        {
            return this.MaxCount == null;
        }
    }

}
