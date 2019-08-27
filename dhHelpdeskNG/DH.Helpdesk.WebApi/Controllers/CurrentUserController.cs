using System;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Common.Converters;
using DH.Helpdesk.Web.Common.Models.CurrentUser;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;

namespace DH.Helpdesk.WebApi.Controllers
{
    [SkipCustomerAuthorization]
    public class CurrentUserController : BaseApiController
    {
        private readonly IUserService _userSerivice;

        public CurrentUserController(IUserService userSerivice)
        {
            _userSerivice = userSerivice;
        }

        /// <summary>
        /// Current user settings.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<UserSettingsModelOutput> Settings()
        {
            var userSettings = await _userSerivice.GetUserOverviewAsync(UserId);
            
            return new UserSettingsModelOutput
            {
                Id = UserId,
                UserGuid = userSettings.UserGUID?.ToString(),
                UserRole = userSettings.UserGroupId,
                CustomerId = userSettings.CustomerId,
                LanguageId = userSettings.LanguageId,
                //note: windows and iana time zones sometimes changes. if unknown timezone is found, update nodatime lib or use other approach.
                TimeZone = userSettings.TimeZoneId.WindowsToIana() ,
                TimeZoneMoment = TimeZoneToMomentConverter.GenerateAddMomentZoneScript(userSettings.TimeZoneId, 2000, DateTime.Now.Year),
                //OwnCasesOnly = userSettings.RestrictedCasePermission.ToBool(),
                CreateCasePermission = userSettings.CreateCasePermission.ToBool(),
                DeleteAttachedFiles = userSettings.DeleteAttachedFilePermission.ToBool()
            };
        }
    }
}