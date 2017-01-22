using System;

namespace AbnfFramework.Converters
{
    public class DelegateConverter<T> : ConverterBase<T>, IValueConverter<T>
    {
        private readonly Func<T, string> _Convert;
        private readonly Func<string, T> _ConvertBack;

        public DelegateConverter(Func<T, string> Convert, Func<string, T> ConvertBack)
        {
            if (Convert == null)
                throw new ArgumentNullException("Convert");

            if (ConvertBack == null)
                throw new ArgumentNullException("ConvertBack");

            this._Convert = Convert;
            this._ConvertBack = ConvertBack;
        }

        public override string ConvertToString(T value)
        {
            return this._Convert(value);
        }

        public override T ConvertBack(string text)
        {
            return this._ConvertBack(text);
        }
    }
}
