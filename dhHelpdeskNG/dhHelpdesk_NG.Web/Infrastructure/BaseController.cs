using DH.Helpdesk.Web.Infrastructure.Cache;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DH.Helpdesk.Web.Infrastructure
{
    //todo: clean namespaces
    using System;
    using System.IO;
    using System.Web.Mvc;
    using System.Linq;
    using System.Web.UI.WebControls;
    using DH.Helpdesk.Services.Exceptions;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure.Attributes;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    //using DH.Helpdesk.Web.Infrastructure.StringExtensions;
    using DH.Helpdesk.Web.Models;
    using System.Configuration;
    using System.Security.Claims;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.BusinessData.Models.ADFS.Input;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Common.Enums;
    using BusinessData.Models.LogProgram;

    [SessionRequired]
    [CustomAuthorize]
    public class BaseController : Controller
    {
        #region Fields

        private readonly IMasterDataService _masterDataService;

        // store settings for customer for all controller to use 
        private Domain.Setting CurrentCustomerSettings { get; set; }

        #endregion

        #region Constructors and Destructors

        public BaseController(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        #endregion

        #region Methods

        //called before a controller action is executed, that is before ~/UserController/index 
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionFacade.CurrentUser != null)
            {
                this.SessionCheck(filterContext);
                this.SetTextTranslation(filterContext);

                ApplicationFacade.RemoveCaseUserInfo(SessionFacade.CurrentUser.Id);
                ApplicationFacade.UpdateLoggedInUserActivity(this.Session.SessionID);
            }
            else if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false))
            {
                throw new Exception("[DEBUG] BusinessLogicException how it can be?");
            }
        }

        //called after a controller action is executed, that is after ~/UserController/index 
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!ControllerContext.HttpContext.Request.IsAjaxRequest() &&
				!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false))
            {
                    //do not use for ajax requests
                this.SetMasterPageModel(filterContext);
                this.AutoDetectTimeZoneMessageCheck();
            }

            base.OnActionExecuted(filterContext);
        }

        private void SessionCheck(ActionExecutingContext filterContext)
        {
            if (SessionFacade.CurrentUser != null)
            {
                SessionFacade.CurrentCustomer =
                    SessionFacade.CurrentCustomer ??
                    this._masterDataService.GetCustomer(SessionFacade.CurrentUser.CustomerId);
            }
        }

        private void SetMasterPageModel(ActionExecutedContext filterContext)
        {
            var masterViewModel = new MasterPageViewModel();
            masterViewModel.Languages = this._masterDataService.GetLanguages();
            masterViewModel.SelectedLanguageId = SessionFacade.CurrentLanguageId;
            masterViewModel.UserPermissions = _masterDataService.GetUserPermissions(SessionFacade.CurrentUser.Id);

            masterViewModel.GlobalSettings = this._masterDataService.GetGlobalSettings();

            if (SessionFacade.CurrentUser != null)
            {
                masterViewModel.Customers = this._masterDataService.GetCustomers(SessionFacade.CurrentUser.Id);
            }
            if (SessionFacade.CurrentCustomer != null)
            {
                masterViewModel.SelectedCustomerId = SessionFacade.CurrentCustomer.Id;
                masterViewModel.CustomerSetting = GetCustomerSettings(SessionFacade.CurrentCustomer.Id);
            }
            this.ViewData[Constants.ViewData.MasterViewData] = masterViewModel;
        }

        private void AutoDetectTimeZoneMessageCheck()
        {
            if (!SessionFacade.WasTimeZoneMessageDisplayed)
            {
                SessionFacade.WasTimeZoneMessageDisplayed = true;
                ViewBag.AutoDetectionResult = SessionFacade.TimeZoneDetectionResult;
            }
        }

        private void SetTextTranslation(ActionExecutingContext filterContext)
        {
            if (this._masterDataService != null)
            {
                //_cache.GetTextTranslations();
                //_cache.GetCaseTranslations();
            }
        }

        protected Domain.Setting GetCustomerSettings(int customerId)
        {
            var settings = _masterDataService.GetCustomerSettings(customerId);
            return settings;
        }

        #region Protected Methods

        /// <summary>
        /// Uses Json.net for serialization json
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected ActionResult JsonDefault(object data, JsonSerializerSettings settings = null)
        {
            return data.ToJsonResult(settings ?? new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        protected JsonResult JsonNet(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        protected string RenderRazorViewToString(string viewName, object model, bool partial = true)
        {
            var viewResult = partial
                ? ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName)
                : ViewEngines.Engines.FindView(this.ControllerContext, viewName, null);

            if (viewResult == null || (viewResult != null && viewResult.View == null))
            {
                throw new FileNotFoundException("View could not be found");
            }

            this.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(
                    this.ControllerContext,
                    viewResult.View,
                    this.ViewData,
                    this.TempData,
                    sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(this.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion

        #endregion
    }
}