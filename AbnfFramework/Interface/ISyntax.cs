using System;

namespace AbnfFramework
{
    public interface ISyntax
    {
        void FormatType<T>(Func<T, string> ConvertToString, Func<string, T> ConvertFromString);

        IEntityBuilder<TObj> Entity<TObj>() where TObj : class;
        IEntityBuilder Entity(Type type);
    }
}
