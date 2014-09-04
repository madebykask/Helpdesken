namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System;

    using DH.Helpdesk.Common.Enums.Settings;

    public class CaseSolutionSettingForUpdate : CaseSolutionSetting
    {
        public CaseSolutionSettingForUpdate(
            int id,
            CaseSolutionFields caseSolutionField,
            CaseSolutionModes caseSolutionMode,
            DateTime changedDate)
            : base(caseSolutionField, caseSolutionMode)
        {
            this.Id = id;
            this.ChangedDate = changedDate;
        }

        public DateTime ChangedDate { get; private set; }
    }
}