namespace DH.Helpdesk.Web.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Web.Infrastructure.Tools;

    public class GlobalController : Controller
    {
        private readonly IReportsHelper reportsHelper;

        public GlobalController(IReportsHelper reportsHelper)
        {
            this.reportsHelper = reportsHelper;
        }

        [HttpGet]
        public FileContentResult GetReportImage(string key)
        {
            var result = this.reportsHelper.GetReportImageFromCache(key);
            return result;
        }
    }
}
