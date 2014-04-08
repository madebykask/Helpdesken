namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class AnalyzeViewModel
    {
        #region Constructors and Destructors

        public AnalyzeViewModel()
        {
        }

        public AnalyzeViewModel(
            ConfigurableFieldModel<SelectList> categories,
            MultiSelectList relatedChanges,
            ConfigurableFieldModel<SelectList> priorities,
            ConfigurableFieldModel<SelectList> responsibles,
            SelectList currencies,
            ConfigurableFieldModel<SelectList> approvalResults,
            AnalyzeModel analyze)
        {
            this.Categories = categories;
            this.RelatedChanges = relatedChanges;
            this.Priorities = priorities;
            this.Responsibles = responsibles;
            this.Currencies = currencies;
            this.ApprovalResults = approvalResults;
            this.Analyze = analyze;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public AnalyzeModel Analyze { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> ApprovalResults { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Categories { get; private set; }

        [LocalizedDisplay("Currency")]
        public SelectList Currencies { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Priorities { get; private set; }

        public MultiSelectList RelatedChanges { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Responsibles { get; private set; }

        #endregion
    }
}