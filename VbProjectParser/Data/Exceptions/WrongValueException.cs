using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data.Exceptions
{
    
    /// <summary>
    /// Thrown when a parameter has a wrong value
    /// </summary>
    public class WrongValueException : Exception
    {
        public readonly string ParameterName;
        public readonly object ActualValue;
        public readonly object ExpectedValue;

        public WrongValueException(string ParameterName, object ActualValue, object ExpectedValue)
            : this(String.Format("Expected parameter {0} to have value {1}, but actual value was {2}", ParameterName, ExpectedValue, ActualValue), ParameterName, ActualValue, ExpectedValue)
        {
        }

        public WrongValueException(string message, string ParameterName, object ActualValue, object ExpectedValue)
            : base(message)
        {
            this.ParameterName = ParameterName;
            this.ActualValue = ActualValue;
            this.ExpectedValue = ExpectedValue;
        }

        public WrongValueException(string message, string ParameterName, object ActualValue, object ExpectedValue, Exception InnerException)
            : base(message, InnerException)
        {
            this.ParameterName = ParameterName;
            this.ActualValue = ActualValue;
            this.ExpectedValue = ExpectedValue;
        }
   
    }
}
