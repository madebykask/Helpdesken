namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    public class GraphicsFields
    {
        public GraphicsFields(string videoCard)
        {
            this.VideoCard = videoCard;
        }

        public string VideoCard { get; set; }
    }
}