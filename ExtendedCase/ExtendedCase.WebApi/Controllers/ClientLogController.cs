using System;
using System.Diagnostics;
using System.Web.Http;
using Common.Logging;
using ExtendedCase.Logic.Services;
using ExtendedCase.Models;

namespace ExtendedCase.WebApi.Controllers
{
    public class ClientLogController : ExtendedCaseApiControllerBase
    {
        private readonly IClientLogService _clientLogService;
        private readonly ILogger _logger;

        #region ctor()

        public ClientLogController()
        {
        }

        public ClientLogController(IClientLogService clientLogService, ILogger logger)
        {
            _clientLogService = clientLogService;
            _logger = logger;
        }

        #endregion

        [HttpPost]
        public IHttpActionResult AddLogItem(ClientLogItemModel model)
        {
            //Debugger.Launch();

            if (!ModelState.IsValid)
            {
                var msg = BuildModelStateErrorSummary(ModelState);
                _logger.Error(msg);
                throw new Exception("Invalid model state");
            }

            _clientLogService.AddLogItem(model);
            return Ok();
        }
    }
}