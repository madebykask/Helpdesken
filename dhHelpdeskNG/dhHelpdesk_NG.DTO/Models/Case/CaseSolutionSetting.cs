namespace DH.Helpdesk.BusinessData.Models.Case
{
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Enums.Settings;
    using DH.Helpdesk.Common.ValidationAttributes;

    public abstract class CaseSolutionSetting : INewBusinessModel
    {
        protected CaseSolutionSetting(
            CaseSolutionFields caseSolutionField,
            CaseSolutionModes caseSolutionMode)
        {
            this.CaseSolutionField = caseSolutionField;
            this.CaseSolutionMode = caseSolutionMode;
        }

        [IsId]
        public int Id { get; set; }

        public CaseSolutionFields CaseSolutionField { get; private set; }

        public CaseSolutionModes CaseSolutionMode { get; private set; }
    }
}
