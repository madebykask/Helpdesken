namespace DH.Helpdesk.Services.DisplayValues.Changes
{
    using System;

    using DH.Helpdesk.BusinessData.Enums.Changes;

    public sealed class StepStatusDisplayValue : DisplayValue<StepStatus>
    {
        public StepStatusDisplayValue(StepStatus value)
            : base(value)
        {
        }

        public static explicit operator StepStatusDisplayValue(StepStatus value)
        {
            var displayValue = new StepStatusDisplayValue(value);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            switch (this.Value)
            {
                case StepStatus.Approved:
                    return "Approved";
                case StepStatus.Rejected:
                    return "Rejected";
                case StepStatus.None:
                    return "None";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}