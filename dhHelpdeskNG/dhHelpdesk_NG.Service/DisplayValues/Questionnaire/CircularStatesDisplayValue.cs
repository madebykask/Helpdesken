namespace DH.Helpdesk.Services.DisplayValues.Questionnaire
{
    using System;

    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Services.Localization;

    public class CircularStatesDisplayValue : DisplayValue<CircularStates>
    {
        public CircularStatesDisplayValue(CircularStates value)
            : base(value)
        {
        }

        public static explicit operator CircularStatesDisplayValue(CircularStates value)
        {
            var displayValue = new CircularStatesDisplayValue(value);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            switch (this.Value)
            {
                case CircularStates.ReadyToSend:
                    return Translator.Translate("Ready to send");
                case CircularStates.Sent:
                    return Translator.Translate("Sent");
                default: 
                    return string.Empty;
            }
        }
    }
}
