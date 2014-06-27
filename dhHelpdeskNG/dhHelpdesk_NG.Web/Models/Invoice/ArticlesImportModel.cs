namespace DH.Helpdesk.Web.Models.Invoice
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public sealed class ArticlesImportModel
    {
        [Required]
        public HttpPostedFileBase File { get; set; }
    }
}