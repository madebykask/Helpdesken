namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;

    public sealed class ServiceController : BaseController
    {
        public ServiceController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
        }

        [HttpPost]
        public void RememberTab(string topic, string tab)
        {
            throw new Exception("Remember tab called.");
        }
    }
}