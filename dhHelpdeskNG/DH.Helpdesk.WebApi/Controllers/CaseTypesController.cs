using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure.Translate;

namespace DH.Helpdesk.WebApi.Controllers
{
    public class CaseTypesController : ApiController
    {
        private readonly ICaseTypeService _caseTypeService;

        public CaseTypesController(ICaseTypeService caseTypeService)
        {
            _caseTypeService = caseTypeService;
        }

        // GET api/<controller>
        public async Task<IEnumerable<CaseTypeOverview>> Get(int cid)
        {
            const bool takeOnlyActive = true;//TODO: move to filter?
            var caseTypes = _caseTypeService.GetCaseTypesOverviewWithChildren(cid, takeOnlyActive)
                .OrderBy(p => p.Name)
                //.OrderBy(c => Translation.GetMasterDataTranslation(c.Name))//TODO: translation for parent and child
                .ToList();//TODO: async

            //var childs = caseType.SubCaseTypes.Where(p => !takeOnlyActive || (p.IsActive != 0 && p.Selectable != 0)).ToList();

            return await Task.FromResult(caseTypes);
        }

        // GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

    }
}