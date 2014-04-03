namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    public class OtherFields
    {
        public OtherFields(ConfigurableFieldModel<string> info)
        {
            this.Info = info;
        }

        public ConfigurableFieldModel<string> Info { get; set; }
    }
}