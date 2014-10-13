namespace DH.Helpdesk.Mobile.Areas.Admin.Models.Invoice
{
    using System.Web;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ArticlesImportModel
    {
        [NotNull]
        public int CustomerId { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}