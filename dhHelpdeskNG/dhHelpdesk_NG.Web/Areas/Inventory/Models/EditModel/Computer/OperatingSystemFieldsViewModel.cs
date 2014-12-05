namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
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