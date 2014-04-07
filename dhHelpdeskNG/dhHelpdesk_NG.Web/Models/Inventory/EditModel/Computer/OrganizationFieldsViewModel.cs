namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsViewModel
    {
        public OrganizationFieldsViewModel(
            OrganizationFieldsModel organizationFieldsModel,
            ConfigurableFieldModel<SelectList> domains,
            ConfigurableFieldModel<SelectList> units)
        {
            this.OrganizationFieldsModel = organizationFieldsModel;
            this.Domains = domains;
            this.Units = units;
        }

        [NotNull]
        public OrganizationFieldsModel OrganizationFieldsModel { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Domains { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Units { get; set; }
    }
}