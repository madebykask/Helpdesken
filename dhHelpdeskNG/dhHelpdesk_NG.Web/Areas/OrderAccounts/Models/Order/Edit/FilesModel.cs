namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit
{
    public sealed class FilesModel
    {
        public FilesModel(string id, string file)
        {
            this.OrderId = id;
            this.File = file;
        }

        public string OrderId { get; private set; }

        public string File { get; private set; }
    }
}