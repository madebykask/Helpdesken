namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System;

    using DH.Helpdesk.Domain.Cases.Settings;

    public class CaseSolutionSettingForInsert : CaseSolutionSetting
    {
        public CaseSolutionSettingForInsert(
            int caseSolutionId,
            CaseSolutionFields caseSolutionField,
            bool isReadonly,
            bool isShow,
            DateTime createdDate)
            : base(caseSolutionField, isReadonly, isShow)
        {
            this.CaseSolutionId = caseSolutionId;
            this.CreatedDate = createdDate;
        }

        public int CaseSolutionId { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}