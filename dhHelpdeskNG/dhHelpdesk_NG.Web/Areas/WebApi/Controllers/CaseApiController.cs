using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Services.Services.UniversalCase;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace DH.Helpdesk.Web.Areas.WebApi
{
    [Authorize(Roles = UserAccessRole.UserRole + "," + 
                       UserAccessRole.AdministratorRole + "," + 
                       UserAccessRole.CustomerAdministratorRole + "," + 
                       UserAccessRole.SystemAdministratorRole)]

    public class CaseApiController : BaseApiController
    {
        private class RequiredParams
        {
            public int? Id { get; set; }
            public int? CurrentCustomerId { get; set; }
            public int? CurrentLanguageId { get; set; }

        }

        private readonly IUniversalCaseService _universalCaseService;
        public CaseApiController(IUniversalCaseService universalCaseService)
        {
            _universalCaseService = universalCaseService;
        }

        [HttpGet]        
        public CaseModel GetCase(int id)
        {                         
            var caseModel = _universalCaseService.GetCase(id);
            return caseModel;
        }
        
        [HttpPost]        
        public string SaveCase([FromBody] JObject jObj)
        {
            #region User Info

            int curUserId = -1;
            var claims = RequestContext.GetClaims(ClaimTypes.Sid, ClaimTypes.Role);
            if (claims.Any())
            {
                if (claims[0] != null)
                    int.TryParse(claims[0], out curUserId);
            }

            if (curUserId == -1)
                return new ProcessResult("SaveCase", ProcessResult.ResultTypeEnum.ERROR, "Could not retrieve user info").Serialize();

            #endregion

            #region Parse parameters

            var dic = new Dictionary<string, object>();         
            try
            {
                dic = jObj.ToObject<Dictionary<string, object>>();               
            }
            catch
            {
                return new ProcessResult("SaveCase", ProcessResult.ResultTypeEnum.ERROR, "Casting error.").Serialize();
            }
                        
            var requiredParams = TryGetRequiredParams(dic);
            if (requiredParams == null)
            {
                return new ProcessResult("SaveCase", ProcessResult.ResultTypeEnum.ERROR,
                   string.Format("Could not retrieve required data! ({0} or {1})",
                   nameof(RequiredParams.Id),
                   nameof(RequiredParams.CurrentLanguageId))).Serialize();
            }

            var errors = new List<KeyValuePair<string, string>>();
            if (!ValidateRequiredParameters(requiredParams, ref errors))
            {
                return new ProcessResult("SaveCase", ProcessResult.ResultTypeEnum.ERROR, "Invalid parameter.", errors).Serialize();
            }

            #endregion

            #region Create Model & Save

            CaseModel caseModel = null;
            if (requiredParams.Id == 0)
                caseModel = new CaseModel();
            else
                caseModel = _universalCaseService.GetCase(requiredParams.Id.Value);

            if (caseModel == null)
                return new ProcessResult("SaveCase", ProcessResult.ResultTypeEnum.ERROR, "Invalid Case id.").Serialize();

            var infoToSave = dic.LoadDictionaryToObject(caseModel);
            var auxModel = new AuxCaseModel(1, curUserId, User.Identity.Name,
                                            RequestExtension.GetAbsoluteUrl(),
                                            CreatedByApplications.Helpdesk5Api);
            int caseId = -1;         
            var res = _universalCaseService.SaveCase(infoToSave, auxModel, out caseId);

            #endregion            

            return res.Serialize();
        }

        private RequiredParams TryGetRequiredParams(Dictionary<string, object> dic)
        {
            var ret = new RequiredParams();
            try
            {                
                ret.Id = dic.GetParamValue<int>(nameof(RequiredParams.Id));
                ret.CurrentLanguageId = dic.GetParamValue<int>(nameof(RequiredParams.CurrentLanguageId));
                ret.CurrentLanguageId = dic.GetParamValue<int>(nameof(RequiredParams.CurrentLanguageId));
            }
            catch
            {
                return null;
            }
            return ret;
        }

        private bool ValidateRequiredParameters(RequiredParams reqParams, ref List<KeyValuePair<string, string>> errors)
        {
            if (!reqParams.Id.HasValue || reqParams.Id < 0)
                errors.Add(GenerateInvalidParameterError(nameof(reqParams.Id)));

            if (!reqParams.CurrentCustomerId.HasValue || reqParams.CurrentCustomerId < 1)
                errors.Add(GenerateInvalidParameterError(nameof(reqParams.CurrentCustomerId)));

            if (!reqParams.CurrentLanguageId.HasValue || reqParams.CurrentLanguageId < 1)
                errors.Add(GenerateInvalidParameterError(nameof(reqParams.CurrentLanguageId)));

            return !errors.Any();
        }

        private KeyValuePair<string, string> GenerateInvalidParameterError(string paramName)
        {
            return new KeyValuePair<string, string>(paramName, string.Format("{0} is not valid.", paramName));
        }  
       
    }    
}
