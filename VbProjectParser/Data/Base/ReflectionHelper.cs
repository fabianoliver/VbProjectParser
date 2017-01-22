using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data
{
    public static class ReflectionHelper
    {
        public static object GetValue(object @obj, MemberInfo member)
        {
            if (@obj == null)
                throw new ArgumentNullException("@obj");

            if (member == null)
                throw new NullReferenceException("member");

            if (member is FieldInfo)
                return ((FieldInfo)member).GetValue(@obj);
            else if (member is PropertyInfo)
                return ((PropertyInfo)member).GetValue(@obj, null);
            else
                throw new InvalidOperationException("Encountered MemberInfo of unknown type");
        }

        public static void SetValue(object @obj, MemberInfo member, object value)
        {
            if (@obj == null)
                throw new NullReferenceException("@obj");

            if (member == null)
                throw new NullReferenceException("member");

            if(member is FieldInfo)
            {
                var fi = (FieldInfo)member;
                fi.SetValue(@obj, value);
            } else if(member is PropertyInfo)
            {
                var pi = (PropertyInfo)member;
                pi.SetValue(@obj, value);
            }
            else
            {
                throw new InvalidOperationException("Encountered MemberInfo of unknown type");
            }
        }

        public static bool IsNumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }

        public static bool AreEqual(object ReferenceValue, object CheckValue)
        {
            if (ReferenceValue == null)
            {
                return CheckValue == null;
            }

            if (IsNumber(ReferenceValue))
            {
                Type ReferenceType = ReferenceValue.GetType();
                CheckValue = Convert.ChangeType(CheckValue, ReferenceType); // e.g. necessary when comparing int to unit
            }

            return ReferenceValue.Equals(CheckValue);
        }

        public static Type GetTypeOf(MemberInfo mi)
        {
            if (mi is PropertyInfo)
                return ((PropertyInfo)mi).PropertyType;
            else if (mi is FieldInfo)
                return ((FieldInfo)mi).FieldType;
            else
                throw new ArgumentException("Unrecognized MemberInfo subtype");
        }

        public static object GetPropertyValue(object obj, string propertyPath)
        {
            object propertyValue = null;
            if (propertyPath.IndexOf(".") < 0)
            {
                var objType = obj.GetType();
                var property = objType.GetProperty(propertyPath);
                var member = objType.GetMember(propertyPath, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).First();
                propertyValue = GetValue(obj, member);
                return propertyValue;
            }
            var properties = propertyPath.Split('.').ToList();
            var midPropertyValue = obj;
            while (properties.Count > 0)
            {
                var propertyName = properties.First();
                properties.Remove(propertyName);
                propertyValue = GetPropertyValue(midPropertyValue, propertyName);
                midPropertyValue = propertyValue;
            }
            return propertyValue;
        }
    }
}
