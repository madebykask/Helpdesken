namespace DH.Helpdesk.Services.DisplayValues
{
    using System;

    public sealed class DateTimeDisplayValue : DisplayValue<DateTime?>
    {
        public DateTimeDisplayValue(DateTime? value)
            : base(value)
        {
        }

        public static explicit operator DateTimeDisplayValue(DateTime? value)
        {
            var displayValue = new DateTimeDisplayValue(value);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            return this.Value.ToString();
        }
    }
}