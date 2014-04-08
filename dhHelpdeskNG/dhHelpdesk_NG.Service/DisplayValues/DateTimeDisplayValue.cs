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

        public override string GetDisplayValue()
        {
            return this.value.ToString();
        }
    }
}