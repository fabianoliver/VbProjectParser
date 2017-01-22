using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data
{
    public class ValueMustEqualMember : ValidationAttribute
    {
        protected readonly string OtherMemberPath;

        public ValueMustEqualMember(string OtherMemberPath)
        {
            this.OtherMemberPath = OtherMemberPath;
        }

        protected virtual object GetOtherMemberValue(object ValidationObject)
        {
            return ReflectionHelper.GetPropertyValue(ValidationObject, this.OtherMemberPath);
        }

        public override ValidationResult Validate(object ValidationObject, MemberInfo member)
        {
            if (ValidationObject == null)
                throw new ArgumentNullException("ValidationObject");

            if (member == null)
                throw new ArgumentNullException("member");

            var ActualValue = ReflectionHelper.GetValue(ValidationObject, member);
            object OtherMemberValue;
            try
            {
                OtherMemberValue = GetOtherMemberValue(ValidationObject);
            } catch (NullReferenceException ex)
            {
                return new ValidationResult(new ArgumentException("Could not access member path " + this.OtherMemberPath, member.Name, ex));
            }
            
            if (!ReflectionHelper.AreEqual(OtherMemberValue, ActualValue))
            {
                var ex = new ArgumentException(
                    String.Format(
                        "Expected {0} to equal the value of member {1}, but {0}={2} and {1}={3}",
                        member.Name, this.OtherMemberPath, ActualValue, OtherMemberValue),
                    member.Name);
                return new ValidationResult(ex);
            }

            return new ValidationResult();
        }
    }
}
