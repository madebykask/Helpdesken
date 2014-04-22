namespace DH.Helpdesk.Services.DisplayValues
{
    using System.Globalization;

    public sealed class IntegerDisplayValue : DisplayValue<int?>
    {
        public IntegerDisplayValue(int? value)
            : base(value)
        {
        }

        public static explicit operator IntegerDisplayValue(int? value)
        {
            var displayValue = new IntegerDisplayValue(value);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            return this.Value == null ? null : this.Value.Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}