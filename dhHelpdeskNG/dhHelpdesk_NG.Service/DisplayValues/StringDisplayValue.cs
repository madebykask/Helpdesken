namespace DH.Helpdesk.Services.DisplayValues
{
    public sealed class StringDisplayValue : DisplayValue
    {
        private readonly string value;

        public StringDisplayValue(string value)
        {
            this.value = value;
        }

        public static explicit operator StringDisplayValue(string value)
        {
            var displayValue = new StringDisplayValue(value);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            return this.value;
        }
    }
}