namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared
{
    public class OrganizationFieldsSettings
    {
        public OrganizationFieldsSettings(int? domain, int? unit)
        {
            this.Domain = domain;
            this.Unit = unit;
        }

        public int? Domain { get; set; }

        public int? Unit { get; set; }
    }
}