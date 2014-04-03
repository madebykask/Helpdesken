namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ImplementationViewModel
    {
        #region Constructors and Destructors

        public ImplementationViewModel()
        {
        }

        public ImplementationViewModel(ConfigurableFieldModel<SelectList> statuses, ImplementationModel implementation)
        {
            this.Status = statuses;
            this.Implementation = implementation;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public ImplementationModel Implementation { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Status { get; private set; }

        #endregion
    }
}