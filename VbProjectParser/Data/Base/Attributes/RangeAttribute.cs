using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data
{
    public class RangeAttribute : ValidationAttribute
    {
        protected Int64 UpperBound;
        protected Int64 LowerBound;

        public RangeAttribute(UInt16 LowerBound, UInt16 UpperBound)
        {
            if (UpperBound < LowerBound)
                throw new ArgumentException("UpperBound must be greater or equal than lower bound", "UpperBound");

            this.LowerBound = Convert.ToInt64(LowerBound);
            this.UpperBound = Convert.ToInt64(UpperBound);
        }

        public RangeAttribute(UInt32 LowerBound, UInt32 UpperBound)
        {
            if (UpperBound < LowerBound)
                throw new ArgumentException("UpperBound must be greater or equal than lower bound", "UpperBound");

            this.LowerBound = Convert.ToInt64(LowerBound);
            this.UpperBound = Convert.ToInt64(UpperBound);
        }

        public RangeAttribute(Int16 LowerBound, Int16 UpperBound)
        {
            if (UpperBound < LowerBound)
                throw new ArgumentException("UpperBound must be greater or equal than lower bound", "UpperBound");

            this.LowerBound = Convert.ToInt64(LowerBound);
            this.UpperBound = Convert.ToInt64(UpperBound);
        }

        public RangeAttribute(Int32 LowerBound, Int32 UpperBound)
        {
            if (UpperBound < LowerBound)
                throw new ArgumentException("UpperBound must be greater or equal than lower bound", "UpperBound");

            this.LowerBound = Convert.ToInt64(LowerBound);
            this.UpperBound = Convert.ToInt64(UpperBound);
        }

        public RangeAttribute(Int64 LowerBound, Int64 UpperBound)
        {
            if (UpperBound < LowerBound)
                throw new ArgumentException("UpperBound must be greater or equal than lower bound", "UpperBound");

            this.LowerBound = Convert.ToInt64(LowerBound);
            this.UpperBound = Convert.ToInt64(UpperBound);
        }

        public override ValidationResult Validate(object ValidationObject, MemberInfo member)
        {
            var ActualValue = ReflectionHelper.GetValue(ValidationObject, member);

            if(!ReflectionHelper.IsNumber(ActualValue))
            {
                var ex = new ArgumentException(String.Format("Expected a numeric type for {0}, however found {1}", member.Name, (ActualValue == null ? "null" : ActualValue.GetType().Name)), member.Name);
                return new ValidationResult(ex);
            }

            Int64 _ActualValue = Convert.ToInt64(ActualValue);
            if(_ActualValue < this.LowerBound || _ActualValue > this.UpperBound)
            {
                var ex = new ArgumentException(String.Format("Expected {0} to be within range {1} to {2}, but was {3}", member.Name, this.LowerBound, this.UpperBound, ActualValue), member.Name);
                return new ValidationResult(ex);
            }

            return new ValidationResult();
        }
    }
}
