using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Concrete;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    public class OrganizationalUnitsController : BaseApiController
    {
        private readonly IOUService _ouService;

        public OrganizationalUnitsController(IOUService ouService)
        {
            _ouService = ouService;
        }

        // GET api/<controller>
        public async Task<IEnumerable<ItemOverview>> Get(int cid, int departmentId)
        {
            var ous = await _ouService.GetActiveOuForDepartmentAsync(departmentId, cid);

            return ous.Select(ou =>
            {
                var name = ou.Name;
                if (ou.Parent_OU_Id.HasValue)
                {
                    var parentName = ous.FirstOrDefault(pou => pou.Id == ou.Parent_OU_Id.Value)?.Name;
                    if(!string.IsNullOrWhiteSpace(parentName)) name = $"{ou.Name} - {parentName}";
                }

                return new ItemOverview(ou.Id.ToString(), name);
            }).ToArray();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

    }
}