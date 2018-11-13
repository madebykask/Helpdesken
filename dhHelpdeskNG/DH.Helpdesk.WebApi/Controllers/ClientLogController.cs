using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;
using DH.Helpdesk.WebApi.Infrastructure.ClientLogger;
using DH.Helpdesk.WebApi.Models;

namespace DH.Helpdesk.WebApi.Controllers
{
    public class ClientLogController : BaseApiController
    {
        private readonly IClientLogger _clientLogger;
        private readonly IMapper _mapper;
        
        #region ctor()

        public ClientLogController(IClientLogger clientLogger, IMapper mapper)
        {
            _clientLogger = clientLogger;
            _mapper = mapper;
        }

        #endregion
        
        /// <summary>
        /// Gets errors from client and stores it.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [SkipCustomerAuthorization]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Post([FromBody]ClientLogItemModel model)
        {
            var logEntry = _mapper.Map<ClientLogItemModel, ClientLogEntry>(model);
            await _clientLogger.LogAsync(logEntry);
            
            return Ok();
        }
    }
}