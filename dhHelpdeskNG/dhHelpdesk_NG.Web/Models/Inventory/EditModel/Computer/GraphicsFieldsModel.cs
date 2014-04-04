namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    public class GraphicsFieldsModel
    {
        public GraphicsFieldsModel(ConfigurableFieldModel<string> videoCard)
        {
            this.VideoCard = videoCard;
        }

        public ConfigurableFieldModel<string> VideoCard { get; set; }
    }
}