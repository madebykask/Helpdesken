namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    public class GraphicsFieldsSettings
    {
        public GraphicsFieldsSettings(string videoCard)
        {
            this.VideoCard = videoCard;
        }

        public string VideoCard { get; set; }
    }
}