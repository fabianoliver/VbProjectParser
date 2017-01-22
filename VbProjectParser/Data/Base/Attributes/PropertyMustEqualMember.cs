using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data
{
    /// <summary>
    /// A property of the member must equal the value of another member of this class
    /// </summary>
    public class PropertyMustEqualMemberAttribute : ValueMustEqualMember
    {
        protected readonly string PropertyName;

        public PropertyMustEqualMemberAttribute(string PropertyName, string OtherMemberName)
            : base(OtherMemberName)
        {
            this.PropertyName = PropertyName;
        }

        public override ValidationResult Validate(object ValidationObject, MemberInfo member)
        {
            if (ValidationObject == null)
                throw new ArgumentNullException("ValidationObject");

            if (member == null)
                throw new ArgumentNullException("member");

            var OtherMemberValue = GetOtherMemberValue(ValidationObject);

            object ActualValue = ReflectionHelper.GetValue(ValidationObject, member);
            var propertyMember = ReflectionHelper.GetTypeOf(member).GetMember(this.PropertyName).First();
            var propertyValue = ReflectionHelper.GetValue(ActualValue, propertyMember);

            if (!ReflectionHelper.AreEqual(OtherMemberValue, propertyValue))
            {
                var ex = new ArgumentException(
                    String.Format(
                        "Expected {0}.{1} to equal the value of member {2}, but {0}.{1}={3} and {2}={4}",
                        member.Name, this.PropertyName, this.OtherMemberPath, propertyValue, OtherMemberValue),
                    member.Name);
                return new ValidationResult(ex);
            }

            return new ValidationResult();
        }

        

        
    }
}
