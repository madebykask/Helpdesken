namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System;

    using DH.Helpdesk.Domain.Cases.Settings;

    public class CaseSolutionSettingForUpdate : CaseSolutionSetting
    {
        public CaseSolutionSettingForUpdate(
            int id,
            CaseSolutionFields caseSolutionField,
            bool isReadonly,
            bool isShow,
            DateTime changedDate)
            : base(caseSolutionField, isReadonly, isShow)
        {
            this.Id = id;
            this.ChangedDate = changedDate;
        }

        public DateTime ChangedDate { get; private set; }
    }
}