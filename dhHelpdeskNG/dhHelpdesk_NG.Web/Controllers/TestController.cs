namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Web;

    using DH.Helpdesk.Services.Exceptions;
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

        public void ErrorHttp(int code, string message)
        {
            throw new HttpException(code, message);
        }

        public void BusinessLogicError(string message)
        {
            throw new ElementaryValidationRulesException("Test Field", message);
        }
    }
}
