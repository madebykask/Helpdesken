namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System;

    using DH.Helpdesk.Common.Enums.Settings;

    public class CaseSolutionSettingForInsert : CaseSolutionSetting
    {
        public CaseSolutionSettingForInsert(
            int caseSolutionId,
            CaseSolutionFields caseSolutionField,
            CaseSolutionModes caseSolutionMode,
            DateTime createdDate)
            : base(caseSolutionField, caseSolutionMode)
        {
            this.CaseSolutionId = caseSolutionId;
            this.CreatedDate = createdDate;
        }

        public int CaseSolutionId { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}