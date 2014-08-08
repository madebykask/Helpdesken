namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsModel
    {
        public OtherFieldsModel()
        {
        }

        public OtherFieldsModel(ConfigurableFieldModel<string> info)
        {
            this.Info = info;
        }

        [NotNull]
        public ConfigurableFieldModel<string> Info { get; set; }
    }
}