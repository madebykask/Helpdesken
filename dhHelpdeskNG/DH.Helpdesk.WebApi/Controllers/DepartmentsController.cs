using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    public class DepartmentsController : BaseApiController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        //[HttpGet]
        //public async Task<IList<ItemOverview>> Get([FromUri] int cid)
        //{
        //    var dep = await Task.FromResult(_departmentService.GetDepartmentsByUserPermissions(UserId, cid));
        //    if (!dep.Any()) dep = await Task.FromResult(_departmentService.GetDepartments(cid));

        //    return dep
        //        .Select(d => new ItemOverview(d.Id.ToString(), Services.utils.ServiceUtils.departmentDescription(d, 1)))//TODO: departmentFilterFormat?
        //        .ToList();
        //}

        [HttpGet]
        //[Route("getitemsbyregion/{cid:int}/{regionId:int?}")]
        public async Task<IList<ItemOverview>> GetByRegion(int cid, int? regionId = null)
        {
            var deps = await Task.FromResult(
                _departmentService.GetActiveDepartmentForUserByRegion(regionId, UserId, cid));

            return deps
                .Select(d => new ItemOverview(Services.utils.ServiceUtils.departmentDescription(d, 0), d.Id.ToString())).ToList();//TODO: departmentFilterFormat?
        }
    }
}