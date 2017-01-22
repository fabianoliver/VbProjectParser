using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Data.Exceptions;

namespace VbProjectParser.Data
{
    public class ValidateWithAttribute : ValidationAttribute
    {
        public delegate ValidationResult ValidationDelegate(object ValidationObject, MemberInfo member);
        //private delegate ValidationDelegate GetDelegate(object ValidationObject, MemberInfo member);

        protected readonly ValidationDelegate InvokeDelegate;

        public ValidateWithAttribute(ValidationDelegate DelegateFunction)
        {
            if (DelegateFunction == null)
                throw new ArgumentNullException("DelegateFunction");

            this.InvokeDelegate = DelegateFunction;
        }

        public ValidateWithAttribute(string MethodName)
        {
            this.InvokeDelegate = (object ValidationResult, MemberInfo member) =>
                {
                    var mi = ValidationResult.GetType().GetMethod(MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                    if (mi == null)
                        throw new NullReferenceException(String.Format("Could not find method '{0}' to validate member '{1}' with",MethodName, member.Name));

                    ValidationResult result = (ValidationResult)mi.Invoke(ValidationResult, new object[] { ValidationResult, member });
                    return result;
                };
        }

        public override ValidationResult Validate(object ValidationObject, MemberInfo member)
        {
            var result = this.InvokeDelegate(ValidationObject, member);
            return result;
        }
    }
}
