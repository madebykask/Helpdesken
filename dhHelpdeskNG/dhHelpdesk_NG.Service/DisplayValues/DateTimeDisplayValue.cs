namespace DH.Helpdesk.Services.DisplayValues
{
    using System;

    public sealed class DateTimeDisplayValue : DisplayValue
    {
        private readonly DateTime? value;

        public DateTimeDisplayValue(DateTime? value)
        {
            this.value = value;
        }

        public static explicit operator DateTimeDisplayValue(DateTime? value)
        {
            var displayValue = new DateTimeDisplayValue(value);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            return this.value.ToString();
        }
    }
}