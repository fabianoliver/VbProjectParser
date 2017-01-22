using System;

namespace AbnfFramework
{
    public interface IValueConverter
    {
        string ConvertToString(object value);
        object ConvertBack(string text, Type TargetType);
    }

    public interface IValueConverter<T> : IValueConverter
    {
        string ConvertToString(T value);
        new T ConvertBack(string text);
    }
}
