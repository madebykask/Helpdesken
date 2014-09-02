namespace DH.Helpdesk.BusinessData.Models.Case
{
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Domain.Cases.Settings;

    public abstract class CaseSolutionSetting : INewBusinessModel
    {
        protected CaseSolutionSetting(CaseSolutionFields caseSolutionField, bool isReadonly, bool isShow)
        {
            this.CaseSolutionField = caseSolutionField;
            this.IsReadonly = isReadonly;
            this.IsShow = isShow;
        }

        public int Id { get; set; }

        public CaseSolutionFields CaseSolutionField { get; private set; }

        public bool IsReadonly { get; private set; }

        public bool IsShow { get; private set; }
    }
}
