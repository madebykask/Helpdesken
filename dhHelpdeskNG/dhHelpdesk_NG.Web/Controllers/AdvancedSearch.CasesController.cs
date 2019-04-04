using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Enums.Users;
using DH.Helpdesk.BusinessData.Models.Grid;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.Services.Services.Grid;
using DH.Helpdesk.Web.Common.Models.CaseSearch;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Models.Case;

namespace DH.Helpdesk.Web.Controllers
{
    public partial class CasesController 
    {
        [System.Web.Http.HttpGet]
        public ActionResult AdvancedSearchNew(bool? clearFilters = false, bool doSearchAtBegining = false, bool isExtSearch = false)
        {
            if (SessionFacade.CurrentUser == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            ApplicationFacade.UpdateLoggedInUser(Session.SessionID, string.Empty);

            if (SessionFacade.CurrentCustomer == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var currentUserId = SessionFacade.CurrentUser.Id;

            var model = new AdvancedSearchIndexViewModel();

            var userCustomers =
                _userService.GetUserProfileCustomersSettings(SessionFacade.CurrentUser.Id)
                     .Select(c => new ItemOverview(c.CustomerName, c.CustomerId.ToString()))
                     .OrderBy(c => c.Name).ToList();

            model.UserCustomers = userCustomers;

            var extendIncludedCustomerIds = _settingService.GetExtendedSearchIncludedCustomers();
            
            //todo: refactor to return only required customer data - along with available customer request
            var extCustomers = 
                _customerService.GetAllCustomers()
                    .Where(x => extendIncludedCustomerIds.Contains(x.Id))
                    .Select(c => new ItemOverview(c.Name, c.Id.ToString()))
                    .OrderBy(c => c.Name).ToList();

            model.ExtendedCustomers = extCustomers;
            
            CaseSearchModel advancedSearchModel;
            if ((clearFilters != null && clearFilters.Value) || SessionFacade.CurrentAdvancedSearch == null)
            {
                SessionFacade.CurrentAdvancedSearch = null;
                var userCustomerSettings = _userService.GetUser(currentUserId);

                advancedSearchModel = 
                    CreateAdvancedSearchModel(currentCustomerId, currentUserId, null, userCustomerSettings.StartPage == (int)StartPage.AdvancedSearch);

                SessionFacade.CurrentAdvancedSearch = advancedSearchModel;
            }
            else
            {
                advancedSearchModel = SessionFacade.CurrentAdvancedSearch;
            }

            model.CaseSearchFilterData = 
                CreateAdvancedSearchFilterData(currentCustomerId, currentUserId, advancedSearchModel, userCustomers);

            model.SpecificSearchFilterData = CreateAdvancedSearchSpecificFilterData(currentUserId);

            model.CaseSetting = GetCaseSettingModel(currentCustomerId, currentUserId);
            
            if (advancedSearchModel.Search != null)
            {
                model.SortOptions = new GridSortOptions
                {
                    sortBy = advancedSearchModel.Search.SortBy,
                    sortDir = advancedSearchModel.Search.Ascending ? SortingDirection.Asc : SortingDirection.Desc
                };
            }

            model.CaseSearchFilterData.IsAboutEnabled =
                model.CaseSetting.ColumnSettingModel.CaseFieldSettings.GetIsAboutEnabled();

            model.DoSearchAtBegining = doSearchAtBegining;
            model.IsExtSearch = isExtSearch;

            return View("AdvancedSearch", model);
        }
    }
}