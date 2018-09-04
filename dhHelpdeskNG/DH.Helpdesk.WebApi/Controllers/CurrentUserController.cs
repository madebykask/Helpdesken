using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.Models.CurrentUser;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    public class CurrentUserController : BaseApiController
    {
        private readonly IUserService _userSerivice;

        public CurrentUserController(IUserService userSerivice)
        {
            _userSerivice = userSerivice;
        }

        [HttpGet]
        public async Task<UserSettingsModelOutput> Settings()
        {
            var userSettings = await Task.FromResult(_userSerivice.GetUserOverview(UserId));//TODO: create new method to get cached/async result.
            return new UserSettingsModelOutput
            {
                CustomerId = userSettings.CustomerId
            };
        }
    }
}