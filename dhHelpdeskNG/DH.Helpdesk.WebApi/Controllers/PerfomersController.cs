using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.Models.User;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/perfomers")]
    public class PerfomersController : BaseApiController
    {
        private readonly ISettingService _customerSettingsService;
        private readonly IUserService _userService;

        public PerfomersController(ISettingService customerSettingsService, IUserService userService)
        {
            _customerSettingsService = customerSettingsService;
            _userService = userService;
        }

        [HttpGet]
        [Route("options")]
        public async Task<List<ItemOverview>> Get(int cid, int? performerUserId = null, int? workingGroupId = null)
        {
            var customerSettings = _customerSettingsService.GetCustomerSettings(cid);
            IList<CustomerUserInfo> performersList;

            if (customerSettings.DontConnectUserToWorkingGroup == 0 && workingGroupId.HasValue)
            {
                //TODO: async
                performersList = _userService.GetAvailablePerformersForWorkingGroup(cid, workingGroupId); 
                if (performerUserId.HasValue && performersList.All(u => u.Id != performerUserId.Value))
                {
                    var currentCaseAdmin = this._userService.GetUserInfo(performerUserId.Value);
                    if (currentCaseAdmin != null)
                    {
                        performersList.Insert(0, currentCaseAdmin);
                    }
                }
            }
            else
            {
                // TODO: async
                performersList = _userService.GetAvailablePerformersOrUserId(cid, performerUserId); 
            }
            
            var items = customerSettings.IsUserFirstLastNameRepresentation == 1
                ? performersList.OrderBy(it => it.FirstName)
                    .ThenBy(it => it.SurName)
                    .Select(u => new ItemOverview($"{u.FirstName} {u.SurName}", u.Id.ToString())).ToList()
                : performersList.OrderBy(it => it.SurName)
                    .ThenBy(it => it.FirstName)
                    .Select(u => new ItemOverview($"{u.SurName} {u.FirstName}", u.Id.ToString())).ToList();

            return items;
        }

        [HttpGet]
        [SkipCustomerAuthorization]
        public IHttpActionResult GetEmailById(int id)
        {
            var email = _userService.GetUserEmail(id);
            return Json(new { eMail = email });
        }
    }
}
