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
using System.Security.Cryptography;
using Autofac.Builder;


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
        /// <param name="customerId">Lorem Ipsum</param>
        /// <param name="languageId">Lorem Ipsum</param>
        /// <param name="email">Lorem Ipsum</param>
        /// <param name="stateSecondaryIds">Lorem Ipsum</param>
        /// <param name="orderByCaseField">Lorem Ipsum</param>
        /// <param name="caseFieldsToReturn">Lorem Ipsum</param>
        /// <param name="limit">The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 20.</param>
        /// <param name="offset">The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
        public CasesResult Cases([FromUri]int customerId, [FromUri] int languageId, [FromUri]string email, [FromUri]string orderByCaseField, [FromUri]string caseFieldsToReturn, [FromUri] int? limit, [FromUri] int? offset)
        {
            if (string.IsNullOrEmpty(caseFieldsToReturn))
            {
                caseFieldsToReturn = "RegistrationDate, DepartmentName, InitiatorName, Subject";
            }

            if (string.IsNullOrEmpty(orderByCaseField))
            {
                orderByCaseField = "RegistrationDate desc";
            }

            ////#12364 - Paginering

            //endast de kunder som har igång webpart
            string[]  acceptedCustomerIds = ConfigurationManager.AppSettings["WebpartAcceptedCustomerIds"].Split(',');

            //#12144 Jag ska bara kunna se mina egna case (de som jag är assignade till)
            bool myCases = true;

            //#12363
            //caseFieldsToReturn (Dessa kommer vi att skicka in)
            //Persons_Name = InitiatorName ?
            //RegTime = RegistrationDate
            //Caption = c.Subject
            //WatchDate WatchDate
            //ReportedBy ?
            //Performer_User_Id
            //CaseId
            //CaseNumber

            //#12385 - skicka in 
            //Id,
            //CaseNumber,
            //Subject,
            //RegistrationDate,
            //WatchDate,
            //InitiatorName,
            //PerformerName



            try
            {
                //Hämta PerformerUserId baserat på emailadressen
                DH.Helpdesk.Domain.User user = _userSerivice.GetUserByEmail(email);
                //
                if (user != null && acceptedCustomerIds.Contains(customerId.ToString()))
                {

                    int from = (offset.HasValue ? offset.Value : 0);
                    int count = (limit.HasValue ? limit.Value : 20);

                    bool orderByAscending = orderByCaseField.ToLower().Contains("desc") ? false : true;
                    orderByCaseField = orderByCaseField.Replace(" desc", "");
                    orderByCaseField = orderByCaseField.Replace("desc", "");
                    var customerCases = _caseSearchService.SearchActiveCustomerUserCases(false, user.Id, customerId, "", from, count, orderByCaseField, orderByAscending);

                    var model = new CasesResult(customerCases, caseFieldsToReturn);

                   

                    foreach (var c in model.Columns)
                    {
                        c.Identifier = c.Name;
                        c.Name = _caseTranslationService.GetCaseTranslation(GetTranslatedColumnName(c.Name), languageId, customerId);
                        
                        if (c.Identifier == "Id")
                        {
                            c.IsVisible = false;
                        }

                        if ((c.Identifier.ToLower().Contains("phone")))
                        {
                            c.IsHref = true;
                            c.HrefType = "tel:";
                        }

                    }

                    return model;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string GetTranslatedColumnName(string columnName)
        {
            switch (columnName)
            {
                case "InitiatorName":
                    return "Persons_Name";
                case "RegistrationDate":
                    return "RegTime";
                case "Subject":
                    return "Caption";
                case "PerformerName":
                    return "Performer_User_Id";
                case "PersonsPhone":
                    return "Persons_Phone";

                    
                default:
                    return columnName;
            }

        }
    }
}
