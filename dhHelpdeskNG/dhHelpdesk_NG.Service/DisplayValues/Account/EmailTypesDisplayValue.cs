using DH.Helpdesk.Domain.Orders;

namespace DH.Helpdesk.Services.DisplayValues.Account
{
    using System;

    using DH.Helpdesk.BusinessData.Enums.Accounts.Fields;
    using DH.Helpdesk.Services.Localization;

    public class EmailTypesDisplayValue : DisplayValue<EMailTypes>
    {
        public EmailTypesDisplayValue(EMailTypes value)
            : base(value)
        {
        }

        public static explicit operator EmailTypesDisplayValue(EMailTypes value)
        {
            var displayValue = new EmailTypesDisplayValue(value);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            switch (this.Value)
            {
                case EMailTypes.Standard:
                    return Translator.Translate(EMailTypes.Standard.ToString());
                case EMailTypes.NoEmail:
                    return Translator.Translate(EMailTypes.NoEmail.ToString());
                case EMailTypes.Expanded:
                    return Translator.Translate(EMailTypes.Expanded.ToString());
                default:
                    return string.Empty;
            }
        }
    }
}
