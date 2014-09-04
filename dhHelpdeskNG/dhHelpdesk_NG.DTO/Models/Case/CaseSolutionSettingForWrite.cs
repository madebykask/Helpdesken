namespace DH.Helpdesk.BusinessData.Models.Case
{
    using DH.Helpdesk.Common.Enums.Settings;

    public class CaseSolutionSettingForWrite : CaseSolutionSetting
    {
        public CaseSolutionSettingForWrite(CaseSolutionFields caseSolutionField, CaseSolutionModes caseSolutionMode)
            : base(caseSolutionField, caseSolutionMode)
        {
        }
    }
}