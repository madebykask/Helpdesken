namespace DH.Helpdesk.Services.DisplayValues
{
    using System.Globalization;

    public sealed class IntegerDisplayValue : DisplayValue
    {
        private readonly int? value;

        public IntegerDisplayValue(int? value)
        {
            this.value = value;
        }

        public static explicit operator IntegerDisplayValue(int? value)
        {
            var displayValue = new IntegerDisplayValue(value);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            return this.value == null ? null : this.value.Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}