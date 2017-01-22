using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data
{
    public abstract class ValidationAttribute : Attribute
    {
        /// <summary>
        /// Validates the Member of the ValidationObject
        /// Should throw an exception if validation fails.
        /// </summary>
        public abstract ValidationResult Validate(object ValidationObject, MemberInfo member);

    
    }
}
