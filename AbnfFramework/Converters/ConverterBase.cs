using System;

namespace AbnfFramework.Converters
{
    public abstract class ConverterBase<T> : IValueConverter<T>
    {
        public abstract string ConvertToString(T value);
        public abstract T ConvertBack(string text);


        string IValueConverter.ConvertToString(object value)
        {
            if (value != null && !(value is T))
                throw new InvalidOperationException(String.Format("This converter can only handle type {0}", typeof(T).Name));

            return ConvertToString((T)value);
        }

        object IValueConverter.ConvertBack(string text, Type TargetType)
        {
            if (TargetType != typeof(T))
                throw new InvalidOperationException(String.Format("Can only handle type", typeof(T).Name));

            return ConvertBack(text);
        }
    }
}
