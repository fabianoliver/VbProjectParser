using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data
{
    public class ValidationResult
    {
        public bool IsValid { get; protected set; }
        public IEnumerable<Exception> Exceptions
        {
            get
            {
                return _Exceptions;
            }
        }

        protected List<Exception> _Exceptions = new List<Exception>();

        /// <summary>
        /// Successful validation
        /// </summary>
        public ValidationResult()
        {
            this.IsValid = true;
        }

        /// <summary>
        /// Unsuccessful validation
        /// </summary>
        public ValidationResult(Exception ex)
        {
            this.IsValid = false;
            this._Exceptions.Add(ex);
        }

        /// <summary>
        /// Unsuccessful validation
        /// </summary>
        public ValidationResult(IEnumerable<Exception> exceptions)
        {
            this.IsValid = false;
            this._Exceptions.AddRange(exceptions);
        }

        public void Merge(ValidationResult Other)
        {
            if (!Other.IsValid)
                this.IsValid = false;

            this._Exceptions.AddRange(Other.Exceptions);
        }
    }
}
