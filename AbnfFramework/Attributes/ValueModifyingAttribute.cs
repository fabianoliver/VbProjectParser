using System;

namespace AbnfFramework.Attributes
{
    public abstract class ValueModifyingAttribute : ModifyingAttribute
    {
        protected Lazy<IValueConverter> ValueConverter { get; set; }

        protected ValueModifyingAttribute(Type ValueConverterType)
        {
            if (ValueConverterType == null)
                throw new ArgumentNullException("ValueConverterType");

            Func<IValueConverter> factory = () => (IValueConverter)Activator.CreateInstance(ValueConverterType);
            this.ValueConverter = new Lazy<IValueConverter>(factory);
        }

        public ValueModifyingAttribute(IValueConverter converterInstance)
        {
            if (converterInstance == null)
                throw new ArgumentNullException("converterInstance");

            Func<IValueConverter> factory = () => converterInstance;
            this.ValueConverter = new Lazy<IValueConverter>(factory);
        }

        protected ValueModifyingAttribute()
        {
            Func<IValueConverter> factory = () => null;
            this.ValueConverter = new Lazy<IValueConverter>(factory);
        }

        protected IValueConverter GetConverterFor(Type type)
        {
            return ValueConverter.Value;
        }
    }
}
