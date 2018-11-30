using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DH.Helpdesk.Models.Case;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Filters;
using DH.Helpdesk.WebApi.Models;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/case")]
    public class CaseLocksController : BaseApiController
    {
        private readonly ICaseLockService _caseLockService;
        private readonly IMapper _mapper;

        #region ctor()

        public CaseLocksController(ICaseLockService caseLockService, IMapper mapper)
        {
            _caseLockService = caseLockService;
            _mapper = mapper;
        }

            #endregion

        // isLocked - already locked by ither
        //todo: add permissions checks
        [HttpPost]
        [CheckUserCasePermissions(CaseIdParamName = "caseId", CheckBody = true)]
        [Route("lock")] //ex: /api/case/lock?cid=1
        public async Task<IHttpActionResult> AcquireCaseLock([FromBody]CaseLockInputModel input)
        {
            // TODO: Permission check if user can do lock (has access to case, other?)
            var model = await GetCaseLockModel(input.CaseId, input.SessionId).ConfigureAwait(false);
            return Ok(model);
        }

        [HttpPost]
        [CheckUserCasePermissions(CaseIdParamName = "caseId", CheckBody = true)] 
        [Route("unlock")] //ex: /api/Case/unlock?cid=1
        public async Task<IHttpActionResult> UnlockCase([FromBody]CaseUnLockInputModel input)
        {
            // TODO: Permission check if user can do lock (has access to case, other?)
            var res = await _caseLockService.UnlockCaseByGUIDAsync(input.LockGuid);
            return Ok(res);
        }

        [HttpPost]
        [CheckUserCasePermissions(CaseIdParamName = "caseId", CheckBody = true)]
        [Route("extendlock")]
        public async Task<IHttpActionResult> ExtendCaseLock([FromBody] ExtendCaseLockInputModel input)
        {
            // TODO: Permission check if user can do lock (has access to case, other?)
            var isSuccess = await _caseLockService.ReExtendLockCaseAsync(input.LockGuid, input.ExtendValue);
            return Ok(isSuccess);
        }

        private async Task<CaseLockModel> GetCaseLockModel(int caseId, string sessionId)
        {
            var caseLock = await _caseLockService.TryAcquireCaseLockAsync(caseId, UserId, sessionId);

            var model = _mapper.Map<CaseLockModel>(caseLock);
            return model;
        }
    }
}