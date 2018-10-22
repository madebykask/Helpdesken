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

        public ClientLogController()
        {
        }

        public ClientLogController(IClientLogger clientLogger, IMapper mapper)
        {
            _clientLogger = clientLogger;
            _mapper = mapper;
        }

        #endregion
        
        [HttpPost]
        [SkipCustomerAuthorization]
        [AllowAnonymous]
        public IHttpActionResult Post([FromBody]ClientLogItemModel model)
        {
            var logEntry = _mapper.Map<ClientLogItemModel, ClientLogEntry>(model);
            _clientLogger.Log(logEntry);
            
            return Ok();
        }
    }
}