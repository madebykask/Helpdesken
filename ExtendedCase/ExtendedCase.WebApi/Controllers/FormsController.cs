using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Common.Logging;
using ExtendedCase.Logic.Services;
using ExtendedCase.Models;
using ExtendedCase.Models.Results;

namespace ExtendedCase.WebApi.Controllers
{
    [RoutePrefix("api/Forms")]
    public class FormsController : ExtendedCaseApiControllerBase
    {
        private readonly IFormService _formService;
        private readonly IHelpdeskCaseSevice _helpdeskCaseSevice;
        private readonly ILogger _logger;

        #region ctor()

        public FormsController()
        {
        }

        public FormsController(IFormService formService, IHelpdeskCaseSevice helpdeskCaseSevice, ILogger logger)
        {
            _formService = formService;
            _helpdeskCaseSevice = helpdeskCaseSevice;
            _logger = logger;
        }

        #endregion

        [HttpGet]
        [Route("Test")]
        public IHttpActionResult Test()
        {
            //Debugger.Launch();
            //var result = await _helpdeskCaseSevice.GetCaseFields(57549);
            return Ok("Hello");
        }

        // GET: api/Forms/5
        /*public ExtendedCaseFormModel Get(int id)
        {
            return _formService.GetFormById(id);
        }*/

        #region MetaData

        // GET: api/Forms/5/MetaData/1
        [HttpGet]
        [Route("{id:int}/MetaData/{languageId:int?}")]
        public IHttpActionResult GetMetaDataById(int id, int? languageId = null)
        {
            var metaDataModel = _formService.GetMetaDataById(id, languageId);
            return Ok(metaDataModel);
        }

        // GET: api/Forms/ByAssignment/MetaData?userRole=1&caseStatus=1&customerId=1&languageId=1
        [HttpGet]
        [Route("ByAssignment/MetaData")]
        public IHttpActionResult GetMetaDataByAssignment(int? userRole = null, int? caseStatus = null, int? customerId = null, int? languageId = null)
        {
            var metaDataModel = _formService.GetMetaDataByAssignment(userRole, caseStatus, customerId, languageId);
            return  Ok(metaDataModel);
        }

        [HttpGet]
        [Route("List")]
        public IHttpActionResult GetFormsList()
        {
            var forms = _formService.GetFormsList();
            return Ok(forms);
        }

        #endregion //MetaData

        #region LoadData

        [HttpGet]
        [Route("{uniqueId:guid}/Data/{caseId:int}")]
        public IHttpActionResult LoadData(Guid uniqueId, int caseId)
        {
            //todo: add error handling

            var result = _formService.GetExtendedCaseDataByUniqueId(uniqueId);

            //todo: load case data via helpdesk api in standalone mode only (when standalone mode is implemented)
            //try
            //{
            //    var caseFields = await _helpdeskCaseSevice.GetCaseFields(caseId);
            //    result.Data.CaseFieldsValues = caseFields;
            //}
            //catch (Exception ex)
            //{
            //    return InternalServerError(ex);
            //}

            return Ok(result);
        }

        #endregion

        #region SaveData

        [HttpPost]
        [Route("Data")]
        public IHttpActionResult SaveData([FromBody] FormDataSaveInputModel saveData)
        {
            //todo: implement Helpdesk api save method call first
            //todo: save case data via helpdesk api in standalone mode only (when standalone mode is implemented)
            //var helpdeskCaseId = saveInput.HelpdeskCaseId;
            //var helpdeskCaseFields = saveInput.CaseFieldsValues;

            if (!ModelState.IsValid)
            {
                var msg = BuildModelStateErrorSummary(ModelState);
                _logger.Error(msg);
                throw new Exception("Invalid model state");
            }

            var formData = _formService.Save(saveData);

            var result = new SaveFormDataResult(formData.Id, formData.ExtendedCaseGuid, formData.ExtendedCaseFormId);
            return Ok(result);
        }

        #endregion
    }
}