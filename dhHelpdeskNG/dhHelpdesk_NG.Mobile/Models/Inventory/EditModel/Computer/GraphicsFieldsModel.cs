namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GraphicsFieldsModel
    {
        public GraphicsFieldsModel()
        {
        }
 
        public GraphicsFieldsModel(ConfigurableFieldModel<string> videoCard)
        {
            this.VideoCard = videoCard;
        }

        [NotNull]
        public ConfigurableFieldModel<string> VideoCard { get; set; }
    }
}