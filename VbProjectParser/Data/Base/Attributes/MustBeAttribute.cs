using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data
{
    public sealed class MustBeAttribute : ValidationAttribute
    {
        /// <summary>
        /// Must be one of these values
        /// </summary>
        public readonly IEnumerable<object> ExpectedValues;

        /// <summary>
        /// Must be of this type
        /// </summary>
        public readonly Type ExpectedType;

        public MustBeAttribute(params UInt32[] values)
            : this(values.Cast<object>(), typeof(UInt32))
        {
        }

        public MustBeAttribute(params UInt16[] values)
            : this(values.Cast<object>(), typeof(UInt16))
        {
        }

        public MustBeAttribute(params Int32[] values)
            : this(values.Cast<object>(), typeof(Int32))
        {
        }

        public MustBeAttribute(params Int16[] values)
            : this(values.Cast<object>(), typeof(Int16))
        {
        }

        public MustBeAttribute(params Byte[] values)
            : this(values.Cast<object>(), typeof(Byte))
        {
        }

        private MustBeAttribute(IEnumerable<object> ExpectedValues, Type ExpectedType)
        {
            this.ExpectedValues = ExpectedValues.ToArray();
            this.ExpectedType = ExpectedType;
        }

        public override ValidationResult Validate(object ValidationObject, MemberInfo member)
        {
            var ActualValue = ReflectionHelper.GetValue(ValidationObject, member);

            if (!this.ExpectedValues.Contains(ActualValue))
            {
                string message = String.Format("Expected {0} to be {1}, but was {2}", member.Name, String.Join(" OR ", this.ExpectedValues), ActualValue);
                return new ValidationResult(new ArgumentException(message, member.Name));
            }

            return new ValidationResult();
        }
    }

}
