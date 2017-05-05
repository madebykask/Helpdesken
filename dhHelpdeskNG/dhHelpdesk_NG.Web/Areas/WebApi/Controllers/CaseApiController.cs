using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Services.Services.UniversalCase;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Models.JsonModels.Case;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
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

            var currentUser = GetCurrentUser();
            var res = _universalCaseService.SaveCase(
                    infoToSave, 
                    new AuxCaseModel(1, currentUser.Item1, TimeZoneInfo.FindSystemTimeZoneById(""), User.Identity.Name)
            );

            return JsonConvert.SerializeObject(res);
        }

        private Tuple<int, string> GetCurrentUser()
        {
            var idt = RequestContext.Principal.Identity as ClaimsIdentity;
            if (idt == null)
                return new Tuple<int, string>(-1, string.Empty);

            var roles = idt.Claims.Where(x => x.Type == ClaimTypes.Role)
                                   .Select(x => x.Value).FirstOrDefault();

            var curUserIdStr = idt.Claims.Where(x => x.Type == ClaimTypes.Sid)
                                   .Select(x => x.Value).FirstOrDefault();

            int curUserId = -1;
            if (curUserIdStr != null)
                int.TryParse(curUserIdStr, out curUserId);

            return new Tuple<int, string>(curUserId, roles);
        }
      
    }    
}
