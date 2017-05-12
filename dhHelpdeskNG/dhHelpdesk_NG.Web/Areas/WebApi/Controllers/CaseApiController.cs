using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Services.Services.UniversalCase;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Models.JsonModels.Case;
using System.Linq;
using System.Security.Claims;
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
            var caseModel = _universalCaseService.GetCase(id).ToJsonModel();
            return caseModel.Serialize();
        }

        [HttpPost]
        [Authorize]
        public string SaveCase([FromBody] CaseJsonModel model)
        {
            var infoToSave = model.ToBussinessModel();

            int curUserId = -1;
            var claims = RequestContext.GetClaims(ClaimTypes.Sid, ClaimTypes.Role);                        
            if (claims.Any())
            {
                if (claims[0] != null)
                    int.TryParse(claims[0], out curUserId);
            }

            if (curUserId == -1)
                return new ProcessResult("SaveCase", ProcessResult.ResultTypeEnum.ERROR, "Could not retrieve user info").Serialize();
            
            var res = _universalCaseService.SaveCase(
                        infoToSave, 
                        new AuxCaseModel(1,
                                         curUserId,                                      
                                         User.Identity.Name, 
                                         RequestExtension.GetAbsoluteUrl(),
                                         CreatedByApplications.Helpdesk5Api)
            );

            return res.Serialize();
        }        

    }    
}
