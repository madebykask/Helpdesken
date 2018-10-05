using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Case.CaseSections;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Services.Services.Cases;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    public class CaseSectionsController : BaseApiController
    {
        private readonly ICaseSectionService _caseSectionService;

        public CaseSectionsController(ICaseSectionService caseSectionService)
        {
            _caseSectionService = caseSectionService;
        }

        // GET api/<controller>
        public async Task<IEnumerable<CaseSectionModel>> Get(int cid, int langId)//TODO: return not CaseSectionModel, but only required fields
        {
            var sections = _caseSectionService.GetCaseSections(cid, langId);
            sections.ForEach(section =>
            {
                if (string.IsNullOrWhiteSpace(section.SectionHeader))
                    section.SectionHeader = _caseSectionService.GetDefaultHeaderName((CaseSectionType)section.SectionType);
            });

            return await Task.FromResult(sections);//TODO: async 
        }

        // GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}


    }
}