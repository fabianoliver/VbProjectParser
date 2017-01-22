using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Helpers.Validation
{
    public class ValidationResult
    {
        private List<ValidationMessage> Messages = new List<ValidationMessage>();

        public static readonly ValidationResult Success = new ValidationResult();

        public ValidationResult()
        {

        }

        public bool IsSuccess()
        {
            return !GetErrors().Any();
        }

        public IEnumerable<ValidationError> GetErrors()
        {
            return Messages.OfType<ValidationError>();
        }

        public IEnumerable<ValidationWarning> GetWarnings()
        {
            return Messages.OfType<ValidationWarning>();
        }

        public IEnumerable<ValidationMessage> GetMessages()
        {
            return Messages.AsReadOnly();
        }

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool DisplayWarnings)
        {
            if(IsSuccess())
            {
                StringBuilder sb = new StringBuilder("Validation ok");

                if (DisplayWarnings)
                {
                    foreach (var warning in GetWarnings())
                    {
                        sb.Append("Warning: " + warning.ToString());
                    }
                }

                return sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder("Validation failed");

                foreach(var message in this.Messages)
                {
                    sb.Append(Environment.NewLine);

                    if(message is ValidationError)
                    {
                        sb.Append("Error: " + message.ToString());
                    } else if(message is ValidationWarning)
                    {
                        sb.Append("Warning: " + message.ToString());
                    }
                    else
                    {
                        sb.Append("Unknown: " + message.ToString());
                    }
                }

                return sb.ToString();
            }
        }

        public ValidationResult Merge(ValidationResult Other)
        {
            return Merge(new ValidationResult[] { Other });
        }

        public ValidationResult Merge(IEnumerable<ValidationResult> Others)
        {
            var result = new ValidationResult();
            result.Messages = this.Messages.Concat(Others.SelectMany(x => x.Messages)).ToList();
            return result;
        }
    }
}
