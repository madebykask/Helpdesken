namespace DH.Helpdesk.Services.DisplayValues
{
    using DH.Helpdesk.Services.Localization;

    public sealed class BooleanDisplayValue : DisplayValue
    {
        private readonly bool value;

        public BooleanDisplayValue(bool value)
        {
            this.value = value;
        }

        public static explicit operator BooleanDisplayValue(bool value)
        {
            var displayValue = new BooleanDisplayValue(value);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            return this.value ? Translator.Translate("Yes") : Translator.Translate("No");
        }
    }
}