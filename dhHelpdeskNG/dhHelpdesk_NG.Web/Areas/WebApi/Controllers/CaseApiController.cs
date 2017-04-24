using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.UniversalCase;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using System.Web.Http;

namespace DH.Helpdesk.Web.Areas.WebApi
{    
    public class CaseApiController : BaseApiController
    {
        private readonly IUniversalCaseService _universalCaseService;
        public CaseApiController(IUniversalCaseService universalCaseService)
        {
            _universalCaseService = universalCaseService;
        }

        [HttpGet]
        [Authorize]
        public string GetCase(int id)
        {
            //var caseModel = _universalCaseService.GetCase(id);
            //return caseModel.ToJson();
            return "yes";
        }
    }    
}
