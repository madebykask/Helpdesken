namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server
{
    public class DocumentFields
    {
        public DocumentFields(string document)
        {
            this.Document = document;
        }

        public string Document { get; set; }

        public static DocumentFields CreateDefault()
        {
            return new DocumentFields(null);
        }
    }
}