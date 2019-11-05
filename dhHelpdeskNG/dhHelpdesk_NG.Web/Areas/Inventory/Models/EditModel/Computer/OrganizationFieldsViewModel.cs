namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
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
            SelectList regions,
            SelectList departments,
            SelectList domains,
            SelectList units)
        {
            this.OrganizationFieldsModel = organizationFieldsModel;
            Regions = regions;
            this.Departments = departments;
            this.Domains = domains;
            this.Units = units;
        }

        [NotNull]
        public OrganizationFieldsModel OrganizationFieldsModel { get; set; }

        [NotNull]
        public SelectList Regions { get; set; }

        [NotNull]
        public SelectList Departments { get; set; }

        [NotNull]
        public SelectList Domains { get; set; }

        [NotNull]
        public SelectList Units { get; set; }
    }
}