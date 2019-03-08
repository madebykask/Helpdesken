using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Notifiers;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Models.Output;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/Notifier")]
    public class NotifierController : BaseApiController
    {
        private readonly IComputerService _computerService;
        private readonly ISettingService _settingService;
        private readonly IDepartmentService _departmentService;

        public NotifierController(IComputerService computerService, 
                                  ISettingService settingService, 
                                  IDepartmentService departmentService)
        {
            _departmentService = departmentService;
            _settingService = settingService;
            _computerService = computerService;
        }

        [HttpGet]
        [Route("Search")]
        [AllowAnonymous] //TODO: REMOVE!!!!
        public Task<IList<UserSearchResults>> Search([FromUri]string query, [FromUri]int cid, [FromUri]int? categoryId = null)
        {
            //todo: make async
            IList<UserSearchResults> result = _computerService.SearchComputerUsers(cid, query, categoryId);

            var customerSettings = _settingService.GetCustomerSettings(cid);
            if (customerSettings.ComputerUserSearchRestriction == 1)
            {
                var departmentIds = _departmentService.GetDepartmentsByUserPermissions(UserId, cid).Select(x => x.Id).ToList();
                //user has no departments checked == access to all departments. TODO: change getdepartmentsbyuserpermissions to actually reflect the "none selected"
                if (departmentIds.Count == 0)
                {
                    departmentIds = _departmentService.GetDepartments(cid).Select(x => x.Id).ToList();
                }

                result = _computerService.SearchComputerUsersByDepartments(cid, query, departmentIds, categoryId);
            }

            return Task.FromResult(result);
        }

        [Route("{id:int}")]
        [HttpGet]
        public async Task<NotifierData> Get(int id)
        {
            var cu = await _computerService.GetComputerUserAsync(id);
            var res = new NotifierData
            {
                Id = cu.Id,
                UserId = cu.UserId,
                Name = (cu.FirstName + ' ' + cu.SurName).Trim(),
                Email = cu.Email,
                Place = cu.Location,
                Phone = cu.Phone,
                UserCode = cu.UserCode,
                Cellphone = cu.Cellphone,
                RegionId = cu.Department?.Region_Id,
                RegionName = cu.Department?.Region?.Name ?? string.Empty,
                DepartmentId = cu.Department_Id,
                DepartmentName = cu.Department?.DepartmentName ?? string.Empty,
                OuId = cu.OU_Id,
                OuName = cu.OU != null ? (cu.OU.Parent != null ? cu.OU.Parent.Name + " - " : string.Empty) + cu.OU.Name : string.Empty,
                CostCentre = string.IsNullOrEmpty(cu.CostCentre) ? string.Empty : cu.CostCentre
            };

            return res;
        }
    }
}