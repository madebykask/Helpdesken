namespace DH.Helpdesk.BusinessData.Models.Case
{
    using DH.Helpdesk.Common.Enums.Settings;

    public class CaseSolutionSettingOverview : CaseSolutionSetting
    {
        public CaseSolutionSettingOverview(int id, CaseSolutionFields caseSolutionField, CaseSolutionModes caseSolutionMode)
            : base(caseSolutionField, caseSolutionMode)
        {
            this.Id = id;
        }
    }
}