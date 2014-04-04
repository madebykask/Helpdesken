namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Shared
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class OperatingSystemFieldsViewModel
    {
        public OperatingSystemFieldsViewModel(
            OperatingSystemFieldsModel operatingSystemFieldsModel,
            ConfigurableFieldModel<SelectList> operatingSystems)
        {
            this.OperatingSystemFieldsModel = operatingSystemFieldsModel;
            this.OperatingSystems = operatingSystems;
        }

        [NotNull]
        public OperatingSystemFieldsModel OperatingSystemFieldsModel { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> OperatingSystems { get; set; }
    }
}