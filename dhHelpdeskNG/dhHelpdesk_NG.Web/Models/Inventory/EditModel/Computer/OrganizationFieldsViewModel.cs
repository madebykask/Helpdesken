namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsViewModel
    {
        public OrganizationFieldsViewModel()
        {
        }

        public OrganizationFieldsViewModel(
            OrganizationFieldsModel organizationFieldsModel,
            SelectList departments,
            SelectList domains,
            SelectList units)
        {
            this.OrganizationFieldsModel = organizationFieldsModel;
            this.Departments = departments;
            this.Domains = domains;
            this.Units = units;
        }

        [NotNull]
        public OrganizationFieldsModel OrganizationFieldsModel { get; set; }

        [NotNull]
        public SelectList Departments { get; set; }

        [NotNull]
        public SelectList Domains { get; set; }

        [NotNull]
        public SelectList Units { get; set; }
    }
}