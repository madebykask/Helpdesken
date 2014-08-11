namespace DH.Helpdesk.Services.DisplayValues.Inventory
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.Services.Localization;

    public class ComputerStatusesDisplayValue : DisplayValue<ComputerStatuses>
    {
        public ComputerStatusesDisplayValue(ComputerStatuses value)
            : base(value)
        {
        }

        public static explicit operator ComputerStatusesDisplayValue(ComputerStatuses value)
        {
            var displayValue = new ComputerStatusesDisplayValue(value);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            switch (this.Value)
            {
                case ComputerStatuses.Active:
                    return Translator.Translate(ComputerStatuses.Active.ToString());
                case ComputerStatuses.Stolen:
                    return Translator.Translate(ComputerStatuses.Stolen.ToString());
                case ComputerStatuses.NotConnectedToUser:
                    return Translator.Translate(ComputerStatuses.NotConnectedToUser.ToString());
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
