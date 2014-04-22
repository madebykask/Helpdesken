namespace DH.Helpdesk.Services.DisplayValues.Inventory
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.Services.Localization;

    public class ContractStatusDisplayValue : DisplayValue<ContractStatuses?>
    {
        public ContractStatusDisplayValue(ContractStatuses? value)
            : base(value)
        {
        }

        public static explicit operator ContractStatusDisplayValue(ContractStatuses? value)
        {
            var displayValue = new ContractStatusDisplayValue(value);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            if (!Value.HasValue)
            {
                return null;
            }

            switch (this.Value)
            {
                case ContractStatuses.Bought:
                    return Translator.Translate(ContractStatuses.Bought.ToString());
                case ContractStatuses.Leasing:
                    return Translator.Translate(ContractStatuses.Leasing.ToString());
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}