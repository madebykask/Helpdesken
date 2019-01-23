using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.Models.User;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;

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
        public async Task<IList<ItemOverview>> Get(int cid, int? performerUserId = null, int? workingGroupId = null)
        {
            var customerSettings = _customerSettingsService.GetCustomerSettings(cid);
            IList<CustomerUserInfo> performersList;

            if (customerSettings.DontConnectUserToWorkingGroup == 0 && workingGroupId.HasValue)
            {
                performersList = await Task.FromResult(_userService.GetAvailablePerformersForWorkingGroup(cid, workingGroupId)); //TODO: async
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
                performersList = await Task.FromResult(_userService.GetAvailablePerformersOrUserId(cid, performerUserId)); // TODO: async
            }

            return customerSettings.IsUserFirstLastNameRepresentation == 1 ?
                    performersList.OrderBy(it => it.FirstName).ThenBy(it => it.SurName)
                        .Select(u => new ItemOverview($"{u.FirstName} {u.SurName}", u.Id.ToString()))
                        .ToList() :
                    performersList.OrderBy(it => it.SurName).ThenBy(it => it.FirstName)
                        .Select(u => new ItemOverview($"{u.SurName} {u.FirstName}", u.Id.ToString()))
                        .ToList();
        }
    }
}
