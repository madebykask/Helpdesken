namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
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