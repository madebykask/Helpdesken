namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Computer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class OperatingSystemFieldsViewModel
    {
        public OperatingSystemFieldsViewModel()
        {
        }

        public OperatingSystemFieldsViewModel(
            OperatingSystemFieldsModel operatingSystemFieldsModel,
            SelectList operatingSystems)
        {
            this.OperatingSystemFieldsModel = operatingSystemFieldsModel;
            this.OperatingSystems = operatingSystems;
        }

        [NotNull]
        public OperatingSystemFieldsModel OperatingSystemFieldsModel { get; set; }

        [NotNull]
        public SelectList OperatingSystems { get; set; }
    }
}