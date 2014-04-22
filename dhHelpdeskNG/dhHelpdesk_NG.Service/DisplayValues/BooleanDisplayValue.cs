namespace DH.Helpdesk.Services.DisplayValues
{
    using DH.Helpdesk.Services.Localization;

    public sealed class BooleanDisplayValue : DisplayValue<bool>
    {
        public BooleanDisplayValue(bool value)
            : base(value)
        {
        }

        public static explicit operator BooleanDisplayValue(bool value)
        {
            var displayValue = new BooleanDisplayValue(value);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            return this.Value ? Translator.Translate("Yes") : Translator.Translate("No");
        }
    }
}