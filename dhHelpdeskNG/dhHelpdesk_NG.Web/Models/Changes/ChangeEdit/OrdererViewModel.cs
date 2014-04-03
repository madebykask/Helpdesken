namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdererViewModel
    {
        #region Constructors and Destructors

        public OrdererViewModel()
        {
        }

        public OrdererViewModel(ConfigurableFieldModel<SelectList> departments, OrdererModel orderer)
        {
            this.Departments = departments;
            this.Orderer = orderer;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public ConfigurableFieldModel<SelectList> Departments { get; private set; }

        [NotNull]
        public OrdererModel Orderer { get; set; }

        #endregion
    }
}