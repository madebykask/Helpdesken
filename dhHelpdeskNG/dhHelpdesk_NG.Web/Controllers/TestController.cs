namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;

    public class TestController : BaseController
    {
        public TestController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
        }

        public void Error(string message)
        {
            throw new Exception(message);
        }
    }
}
