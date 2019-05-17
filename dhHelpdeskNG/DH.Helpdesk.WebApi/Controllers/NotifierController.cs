using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models;
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
        public async Task<List<NotifierSearchItem>> Search([FromUri]string query, [FromUri]int cid, [FromUri]int? categoryId = null)
        {
            //todo: make db methods async
            IList<UserSearchResults> searchResults;
            var settings = await _settingService.GetCustomerSettingsAsync(cid);
            var customerSettings = settings;
            if (customerSettings.ComputerUserSearchRestriction == 1)
            {
                var departmentIds = 
                    _departmentService.GetDepartmentsByUserPermissions(UserId, cid).Select(x => x.Id).ToList();

                //user has no departments checked == access to all departments. TODO: change getdepartmentsbyuserpermissions to actually reflect the "none selected"
                if (departmentIds.Count == 0)
                    departmentIds = _departmentService.GetDepartments(cid).Select(x => x.Id).ToList();

                searchResults =
                    _computerService.SearchComputerUsersByDepartments(cid, query, departmentIds, categoryId);
            }
            else
            {
                searchResults = _computerService.SearchComputerUsers(cid, query, categoryId);
            }

            var result = searchResults.Select(x => new NotifierSearchItem()
            {
                Id = x.Id,
                UserId = x.UserId,
                FirstName = x.FirstName,
                SurName = x.SurName,
                Phone = x.Phone,
                Email = x.Email,
                DepartmentId = x.Department_Id ?? 0,
                DepartmentName = x.DepartmentName,
                UserCode = x.UserCode
            }).ToList();

            return result;
        }
        
        [HttpGet]
        [Route("{id:int}")]
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