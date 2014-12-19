namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit
{
    public sealed class FilesModel
    {
        public FilesModel()
        {
        }

        public FilesModel(string orderId, string file)
        {
            this.OrderId = orderId;
            this.File = file;
        }

        public string OrderId { get; private set; }

        public string File { get; private set; }
    }
}