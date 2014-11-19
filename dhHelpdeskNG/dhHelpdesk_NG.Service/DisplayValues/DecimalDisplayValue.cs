namespace DH.Helpdesk.Services.DisplayValues
{
    using System.Globalization;

    public sealed class DecimalDisplayValue : DisplayValue<decimal>
    {
        public DecimalDisplayValue(decimal value)
            : base(value)
        {
        }

        public static explicit operator DecimalDisplayValue(decimal value)
        {
            return new DecimalDisplayValue(value);
        }

        public override string GetDisplayValue()
        {
            return this.Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}