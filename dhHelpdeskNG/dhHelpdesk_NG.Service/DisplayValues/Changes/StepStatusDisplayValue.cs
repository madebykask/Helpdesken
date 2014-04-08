namespace DH.Helpdesk.Services.DisplayValues.Changes
{
    using System;

    using DH.Helpdesk.BusinessData.Enums.Changes;

    public sealed class StepStatusDisplayValue : DisplayValue
    {
        private readonly StepStatus value;

        public StepStatusDisplayValue(StepStatus value)
        {
            this.value = value;
        }

        public override string GetDisplayValue()
        {
            switch (this.value)
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