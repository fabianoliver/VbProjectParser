using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;

namespace VbProjectParser.Data
{
    public sealed class AutoReadAttribute : System.Attribute
    {
        public readonly int Order;

        protected readonly bool IsArray;
        protected readonly Func<object, MemberInfo, int> CalculateArraySize;

        /// <summary>
        /// The order in which attributes are to be read.
        /// 1 = First to read.
        /// 2 = Second to read.
        /// Etc.
        /// </summary>
        public AutoReadAttribute (int Order)
	    {
            this.Order = Order;
	    }

        public AutoReadAttribute (int Order, int ArraySize)
            : this(Order)
	    {
            this.IsArray = true;
            this.CalculateArraySize = (a,b) => ArraySize;
	    }

        /// <param name="ArraySizeMemberName">Path to a member that provides the size of the array</param>
        public AutoReadAttribute (int Order, string ArraySizeMemberName)
            : this(Order)
        {
            this.IsArray = true;
            this.CalculateArraySize = (object TargetObject, MemberInfo mi) =>
                {
                    var value = ReflectionHelper.GetPropertyValue(TargetObject, ArraySizeMemberName);

                    if (!ReflectionHelper.IsNumber(value))
                        throw new InvalidCastException("Value must be a numeric type");

                    int convertedValue = Convert.ToInt32(value);
                    return convertedValue;
                };
        }

        public object AutoReadValue(XlBinaryReader Reader, object TargetObject, MemberInfo member)
        {
            if (!this.IsArray)
            {
                Type type = ReflectionHelper.GetTypeOf(member);
                return Reader.Read(type);
            }
            else
            {
                var arraySize = this.CalculateArraySize(TargetObject, member);
                Type type = ReflectionHelper.GetTypeOf(member);
                Type elementType = type.GetElementType();
                return Reader.ReadArray(elementType, arraySize);
            }
        }
    }
}
