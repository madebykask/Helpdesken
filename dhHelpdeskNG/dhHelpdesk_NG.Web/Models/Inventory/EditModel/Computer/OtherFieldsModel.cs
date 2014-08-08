namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    public class OtherFieldsModel
    {
        public OtherFieldsModel(ConfigurableFieldModel<string> info)
        {
            this.Info = info;
        }

        public ConfigurableFieldModel<string> Info { get; set; }
    }
}