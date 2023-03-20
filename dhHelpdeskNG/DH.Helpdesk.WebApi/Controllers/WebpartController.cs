using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Enums.Case.Fields;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Paging;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.Models.Shared.Output;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Common.Enums.Case;
using DH.Helpdesk.Web.Common.Extensions;
using DH.Helpdesk.Web.Common.Models.Case;
using DH.Helpdesk.Web.Common.Models.CaseSearch;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.Lists;
using DH.Helpdesk.Models.CasesOverview;
using DH.Helpdesk.WebApi.Infrastructure.Authentication;
using DH.Helpdesk.WebApi.Logic.Case;
using DH.Helpdesk.WebApi.Models.Output;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Services.Utils;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.WebApi.Models;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using System.Runtime.InteropServices.WindowsRuntime;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;
using static DH.Helpdesk.WebApi.Models.CaseOverviewWebpartModel;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/webpart")]
    public class WebpartController : BaseApiController
    {
        private readonly ICaseSearchService _caseSearchService;
        private readonly ICustomerUserService _customerUserService;
        private readonly ICaseSettingsService _caseSettingService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly IUserService _userSerivice;
        private readonly ICustomerService _customerService;
        private readonly ICaseTranslationService _caseTranslationService;
		private readonly ICaseService _caseService;
		private readonly IHolidayService _holidayService;

		public WebpartController(ICaseSearchService caseSearchService,
            ICustomerUserService customerUserService,
            ICaseSettingsService caseSettingService,
            ICaseFieldSettingService caseFieldSettingService,
            ICaseTranslationService caseTranslationService,
            IUserService userSerivice,
            ICustomerService customerService,
			ICaseService caseService,
			IHolidayService holidayService)
        {
            _caseTranslationService = caseTranslationService;
            _caseSearchService = caseSearchService;
            _customerUserService = customerUserService;
            _caseSettingService = caseSettingService;
            _caseFieldSettingService = caseFieldSettingService;
            _userSerivice = userSerivice;
            _customerService = customerService;
			_caseService = caseService;
			_holidayService = holidayService;

		}

        [HttpGet]
        //#12144 - Ta emot secret key "header" jämför med "WebpartSecretKey"
        [WebpartSecretKeyHeader]
        [Route("cases")]
        public CasesResult Cases([FromUri]int customerId, [FromUri]string email, [FromUri]string stateSecondaryIds, [FromUri]string orderByCaseField, [FromUri]string caseFieldsToReturn)
        {
            //#12144 Jag ska bara kunna se mina egna case (de som jag är assignade till) = email kopplat till performer_userId
            //Hämta PerformerUserId baserat på emailadressen

            //Har PerformerUserId tillgång till customerId som skickas in?


            //#12364 - Paginering


            //#12363
            //caseFieldsToReturn (Dessa kommer vi att skicka in)
            //Persons_Name
            //RegTime
            //Caption
            //Performer_User_Id
            //WatchDate
            //ReportedBy
            //CaseId
            //CaseNumber

            //Hämta kolumnerna i den ordningen som användaren har valt

            var casesResult = new CasesResult();

            return casesResult;
        }
    }
}
