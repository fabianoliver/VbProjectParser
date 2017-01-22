using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data.Exceptions;

namespace VbProjectParser.Data
{
    public abstract class DataBase
    {
        public DataBase()
        {
        }

        /// <summary>
        /// This constructor automatically reads data marked with the [AutoRead] attribute
        /// </summary>
        /// <param name="Reader"></param>
        public DataBase(XlBinaryReader Reader)
        {
            AutoReadMembers(Reader);
        }

        protected virtual void AutoReadMembers(XlBinaryReader Reader)
        {
            Type type = thisType;
            var autoReadMembers = type.GetMembers().Where(mi => Attribute.IsDefined(mi, typeof(AutoReadAttribute))).OrderBy(mi => mi.GetCustomAttribute<AutoReadAttribute>().Order);

            foreach (var member in autoReadMembers)
            {
                AutoReadAttribute attr = member.GetCustomAttribute<AutoReadAttribute>();
                var value = attr.AutoReadValue(Reader, this, member);

                ReflectionHelper.SetValue(this, member, value);
            }
        }

        protected virtual void ValidateArraySize<T>(string ParamName, ref T[] array, UInt32 ExpectedSize)
        {
            if(array.Length != Convert.ToInt32(ExpectedSize))
            {
                throw new WrongValueException(ParamName + ".Length", array.Length, Convert.ToInt32(ExpectedSize));
            }
        }

        /// <summary>
        /// Validates all properties of the class
        /// </summary>
        private Type _thisType;
        private Type thisType
        {
            get
            {
                if (_thisType == null)
                    _thisType = this.GetType();

                return _thisType;
            }
        }

        protected virtual void Validate()
        {
            // Must be attribute
            var members = thisType.GetMembers().Where(mi => Attribute.IsDefined(mi, typeof(ValidationAttribute)));

            ValidationResult result = new ValidationResult();

            foreach (var member in members)
            {
                result.Merge(ValidateMember(member, false));
            }

            if(!result.IsValid)
            {
                string message = "Validation failed: " + Environment.NewLine + String.Join(Environment.NewLine, result.Exceptions.Select(ex => ex.Message));
                throw new AggregateException(message, result.Exceptions);
            }
        }

        protected virtual ValidationResult ValidateMember(MemberInfo member, bool throwExceptionOnInvalid = false)
        {
            var result = new ValidationResult();

            var attributes = member.GetCustomAttributes<ValidationAttribute>();
            foreach (var attribute in attributes)
            {
                var validation = attribute.Validate(this, member);
                result.Merge(validation);
            }

            if (throwExceptionOnInvalid && !result.IsValid)
            {
                string message = "Validation failed: " + Environment.NewLine + String.Join(Environment.NewLine, result.Exceptions.Select(ex => ex.Message));
                throw new AggregateException(message, result.Exceptions);
            }

            return result;
        }

        protected object GetValue(MemberInfo mi)
        {
            if (mi is FieldInfo)
                return ((FieldInfo)mi).GetValue(this);
            else if (mi is PropertyInfo)
                return ((PropertyInfo)mi).GetValue(this, null);
            else
                throw new InvalidOperationException("Encountered MemberInfo of unknown type");
        }
    }
}
