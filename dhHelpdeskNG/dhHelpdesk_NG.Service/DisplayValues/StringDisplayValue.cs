namespace DH.Helpdesk.Services.DisplayValues
{
    public class StringDisplayValue : DisplayValue<string>
    {
        public StringDisplayValue(string value)
            : base(value)
        {
        }

        public static explicit operator StringDisplayValue(string value)
        {
            var displayValue = new StringDisplayValue(value);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            return this.Value;
        }
    }


}