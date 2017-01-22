using AbnfFramework.Converters;
using System;
using System.Collections.Generic;

namespace AbnfFramework
{
    public class Syntax : ISyntax
    {
        public readonly AggregateConverter DefaultConverter = new AggregateConverter();
        private readonly IDictionary<Type, IEntityBuilder> _SyntaxBuilders = new Dictionary<Type, IEntityBuilder>();

        public IEntityBuilder<TObj> Entity<TObj>()
            where TObj : class
        {
            Type type = typeof(TObj);

            if (!_SyntaxBuilders.ContainsKey(type))
            {
                var builder = new EntityBuilder<TObj>(this, this.DefaultConverter);
                _SyntaxBuilders.Add(type, builder);
            }

            return (EntityBuilder<TObj>)_SyntaxBuilders[type];
        }

        public IEntityBuilder Entity(Type type)
        {
            if (_SyntaxBuilders.ContainsKey(type))
                return _SyntaxBuilders[type];
            else
                return null;
        }

        public void FormatType<T>(Func<T, string> ConvertToString, Func<string, T> ConvertFromString)
        {
            var converter = new DelegateConverter<T>(ConvertToString, ConvertFromString);
            this.DefaultConverter.Add(converter);
        }
    }

}
