namespace DH.Helpdesk.BusinessData.Models.Case
{
    using DH.Helpdesk.Domain.Cases.Settings;

    public class CaseSolutionSettingOverview : CaseSolutionSetting
    {
        public CaseSolutionSettingOverview(int id, CaseSolutionFields caseSolutionField, bool isReadonly, bool isShow)
            : base(caseSolutionField, isReadonly, isShow)
        {
            this.Id = id;
        }
    }
}