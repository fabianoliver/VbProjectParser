using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data
{
    public class IsEvenNumber : ValidationAttribute
    {
        public override ValidationResult Validate(object ValidationObject, MemberInfo member)
        {
            var value = ReflectionHelper.GetValue(ValidationObject, member);
            bool isEven = false;

            if (value is sbyte)
                isEven = ((sbyte)value) % 2 == 0;
            else if (value is byte)
                isEven = ((byte)value) % 2 == 0;
            else if (value is short)
                isEven = ((short)value) % 2 == 0;
            else if (value is ushort)
                isEven = ((ushort)value) % 2 == 0;
            else if (value is int)
                isEven = ((int)value) % 2 == 0;
            else if (value is uint)
                isEven = ((uint)value) % 2 == 0;
            else if (value is long)
                isEven = ((long)value) % 2 == 0;
            else if (value is ulong)
                isEven = ((ulong)value) % 2 == 0;
            else if (value is float)
                isEven = ((float)value) % 2 == 0;
            else if (value is double)
                isEven = ((double)value) % 2 == 0;
            else if (value is decimal)
                isEven = ((decimal)value) % 2 == 0;
            else
                return new ValidationResult(new ArgumentException("Member was non-numeric; type was " + ((value == null) ? "null" : value.GetType().Name), member.Name));

            if(!isEven)
                return new ValidationResult(new ArgumentException("Member was not even", member.Name));

            return new ValidationResult();
        }
    }
}
