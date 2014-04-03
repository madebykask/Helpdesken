namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    public class GraphicsFields
    {
        public GraphicsFields(ConfigurableFieldModel<string> videoCard)
        {
            this.VideoCard = videoCard;
        }

        public ConfigurableFieldModel<string> VideoCard { get; set; }
    }
}