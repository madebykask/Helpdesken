namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Printer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsViewModel
    {
        public OrganizationFieldsViewModel(
            OrganizationFieldsModel organizationFieldsModel,
            SelectList departments)
        {
            this.OrganizationFieldsModel = organizationFieldsModel;
            this.Departments = departments;
        }

        [NotNull]
        public OrganizationFieldsModel OrganizationFieldsModel { get; set; }

        [NotNull]
        public SelectList Departments { get; set; }
    }
}