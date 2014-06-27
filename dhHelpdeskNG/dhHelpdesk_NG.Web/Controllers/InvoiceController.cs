namespace DH.Helpdesk.Web.Controllers
{
    using System.IO;
    using System.Web.Mvc;

    using DH.Helpdesk.Services.BusinessLogic.Invoice;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Models.Invoice;

    public class InvoiceController : BaseController
    {
        public InvoiceController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
        }

        [HttpGet]
        public ViewResult IkeaArticlesImport()
        {
            return this.View();
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult IkeaArticlesImport(ArticlesImportModel model)
        {
            using (var ms = new MemoryStream())
            {
                var importer = new IkeaExcelImporter();
                var articles = importer.ImportArticles(ms);
            }

            return this.View(model);
        }
    }
}
