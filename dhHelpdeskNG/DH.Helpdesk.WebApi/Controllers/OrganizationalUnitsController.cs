using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/organizationalunits")]
    public class OrganizationalUnitsController : BaseApiController
    {
        private readonly IOUService _ouService;
        private readonly IDepartmentService _departmentService;

        public OrganizationalUnitsController(IOUService ouService, IDepartmentService departmentService)
        {
            _ouService = ouService;
            _departmentService = departmentService;
        }

        /// <summary>
        /// List of Organizational Units filterd by department
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [Route("options")]
        public async Task<IEnumerable<ItemOverview>> Get(int cid, int departmentId)
        {
            var ous = await _ouService.GetActiveOuForDepartmentAsync(departmentId, cid);

            return ous.Select(ou =>
            {
                var name = ou.Name;
                if (ou.Parent_OU_Id.HasValue)
                {
                    var parentName = ous.FirstOrDefault(pou => pou.Id == ou.Parent_OU_Id.Value)?.Name;
                    if (!string.IsNullOrWhiteSpace(parentName)) name = $"{ou.Name} - {parentName}";
                }

                return new ItemOverview(name, ou.Id.ToString());
            }).ToArray();
        }

        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetOU(int id, int cid, int langId)
        {
            if (id > 0)
            {
                var ou = _ouService.GetOU(id);
                if (ou.Department_Id.HasValue)
                {
                    //Check if the user has restrictions on departments - inverted logic..
                    IList<Department> userDepartments = _departmentService.GetDepartmentsByUserPermissions(UserId, cid, false);
                    if (userDepartments.Count > 0 && !userDepartments.Any(d => d.Id == ou.Department_Id.Value))
                    {
                        return Forbidden($"User has no access to ou department");
                    }
                }
                return Ok(new
                {
                    id,
                    parentId = ou.Parent_OU_Id,
                    name = ou.Name
                });
            }

            return Ok();
        }
    }
}