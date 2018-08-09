using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Web.Models.Shared;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Users;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Accounts;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.utils;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Attributes;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Areas.Admin.Infrastructure.Mappers;

    using UserGroup = DH.Helpdesk.BusinessData.Enums.Admin.Users.UserGroup;
    using DH.Helpdesk.BusinessData.Models.Case.CaseLock;

    public class UsersController : BaseController
    {
        private readonly IAccountActivityService _accountActivityService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerUserService _customerUserService;
        private readonly IDepartmentService _departmentService;
        private readonly IDomainService _domainService;
        private readonly ILanguageService _languageService;
        private readonly IOrderTypeService _orderTypeService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ICaseSettingsService _caseSettingsService;
        private readonly ICaseLockService _caseLockService;
        private readonly ISettingService _settingService;

        public UsersController(
            IAccountActivityService accountActivityService,
            ICustomerService customerService,
            ICustomerUserService customerUserService,
            IDepartmentService departmentService,
            IDomainService domainService,
            ILanguageService languageService,
            IOrderTypeService orderTypeService,
            IUserService userService,
            IWorkingGroupService workingGroupService,
            ICaseSettingsService caseSettingsService,
            ICaseLockService caseLockService,
            IMasterDataService masterDataService,
            ISettingService settingService)
            : base(masterDataService)
        {
            this._accountActivityService = accountActivityService;
            this._customerService = customerService;
            this._customerUserService = customerUserService;
            this._departmentService = departmentService;
            this._domainService = domainService;
            this._languageService = languageService;
            this._orderTypeService = orderTypeService;
            this._userService = userService;
            this._workingGroupService = workingGroupService;
            this._caseSettingsService = caseSettingsService;
            this._caseLockService = caseLockService;
            this._settingService = settingService;
        }

        [CustomAuthorize(Roles = "3,4")]
        public ActionResult RemoveUserFromCase(int userId, int caseId, string sessionId)
        {
            ApplicationFacade.RemoveUserFromCase(userId, caseId, sessionId);

            SessionFacade.ActiveTab = "#fragment-2";

            return RedirectToAction("index");
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [CustomAuthorize(Roles = "3,4")]
        public ActionResult Index()
        {
            var model = this.IndexInputViewModel();
            var userSearch = (UserSearch)this.Session["UserSearch"];
           
            if (this.Session["UserSearch"] == null)
            {
                if (SessionFacade.CurrentUser.UserGroupId == 3)
                {
                    model.UserOverviewList.Users = this._userService.GetUsersByUserGroup(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.UserID).ToList();
                }
                else
                {
                    model.UserOverviewList.Users = this._userService.GetUsers(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.UserID).ToList();
                }
                //model.UserOverviewList.Users = this._userService.GetUsers(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.UserID).ToList();
                //model.UserOverviewList.Users = this._userService.GetUsers().OrderBy(x => x.UserID).ToList();
                model.UserOverviewList.Sorting = new UserSort { FieldName = "", IsAsc = true };
                //model.Users = this._userService.GetUsers(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.UserID).ToList();
            }
            else
            {                       
                var filter = (UserSearch)this.Session["UserSearch"];
                model.Filter = filter;
                if (SessionFacade.CurrentUser.UserGroupId == 3)
                {
                    model.UserOverviewList.Users = this._userService.SearchSortAndGenerateUsersByUserGroup(filter);
                }
                else
                {
                    model.UserOverviewList.Users = this._userService.SearchSortAndGenerateUsers(filter);
                }
                
                model.UserOverviewList.Sorting = new UserSort { FieldName = "", IsAsc = true };
                model.StatusUsers.FirstOrDefault(x => x.Value == filter.StatusId.ToString()).Selected = true;               
            }
            
            return this.View(model);
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        public ActionResult Index(UserSearch filter)
        {
            var model = this.IndexInputViewModel();  
            var userSearch = (UserSearch) this.Session["UserSearch"]; 
            
            if (this.Session["UserSearch"] == null && filter.SearchUs == null)
            {
                model.UserOverviewList.Users = this._userService.GetUsers(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.UserID).ToList();
                model.UserOverviewList.Sorting = new UserSort { FieldName = "", IsAsc = true };
                //model.Users = this._userService.GetUsers(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.UserID).ToList();
            }

            if (this.Session["UserSearch"] == null && filter.SearchUs != null)
            {
                model.UserOverviewList.Users = this._userService.SearchSortAndGenerateUsers(filter);
                model.UserOverviewList.Sorting = new UserSort { FieldName = "", IsAsc = true };
            }
            else
            {

                model.Filter = filter;
                this.Session["UserSearch"] = filter;
                if (SessionFacade.CurrentUser.UserGroupId == 3)
                {
                    model.UserOverviewList.Users = this._userService.SearchSortAndGenerateUsersByUserGroup(filter);
                }
                else
                {
                    model.UserOverviewList.Users = this._userService.SearchSortAndGenerateUsers(filter);
                }
            }

           if (userSearch != null)
           {
              var fieldName = userSearch.SortBy;
              var isAsc = userSearch.Ascending;
              model.UserOverviewList.Users = this.SortUsers(model.UserOverviewList.Users, fieldName, isAsc);
              model.UserOverviewList.Sorting = new UserSort { FieldName = fieldName, IsAsc = !isAsc };
           }
            return this.View(model);
        }

        [HttpGet]
        public PartialViewResult DoSort(int customerId, string searchUs, int statusId, string fieldName, bool isAsc)
        {
            var model = new UserList();
            var filter = new UserSearch { CustomerId = customerId, SearchUs = searchUs, StatusId = statusId };
            if (this.Session["UserSearch"] == null && filter.SearchUs == null)
            {
                model.Users = this._userService.GetUsers().OrderBy(x => x.UserID).ToList();
                model.Sorting = new UserSort { FieldName = fieldName, IsAsc = !isAsc };
                //model.Users = this._userService.GetUsers(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.UserID).ToList();
            }
            if (this.Session["UserSearch"] == null && filter.SearchUs != null)
            {
                model.Users = this._userService.SearchSortAndGenerateUsers(filter);
                model.Sorting = new UserSort { FieldName = fieldName, IsAsc = !isAsc };
            }
            else
            {                
                model.Users = this._userService.SearchSortAndGenerateUsers(filter);
            }

            model.Users = this.SortUsers(model.Users,fieldName,isAsc);
            model.Sorting = new UserSort { FieldName = fieldName, IsAsc = !isAsc };

            return PartialView("Index/_UserGrid", model);
        }

        private IEnumerable<User> SortUsers(IEnumerable<User> users, string fieldName, bool  isAsc)
        {
            switch (fieldName)
            {
                case "UserId":
                    if (isAsc)
                        return users.OrderBy(u => u.UserID).ToList();
                    else
                        return users.OrderByDescending(u => u.UserID).ToList();
                case "FirstName":
                    if (isAsc)
                        return users.OrderBy(u => u.FirstName).ToList();
                    else
                        return users.OrderByDescending(u => u.FirstName).ToList();
                case "SurName":
                    if (isAsc)
                        return users.OrderBy(u => u.SurName).ToList();
                    else
                        return users.OrderByDescending(u => u.SurName).ToList();
                case "Phone":
                    if (isAsc)
                        return users.OrderBy(u => u.Phone).ToList();
                    else
                        return users.OrderByDescending(u => u.Phone).ToList();
                case "CellPhone":
                    if (isAsc)
                        return users.OrderBy(u => u.CellPhone).ToList();
                    else
                        return users.OrderByDescending(u => u.CellPhone).ToList();
                case "Email":
                    if (isAsc)
                        return users.OrderBy(u => u.Email).ToList();
                    else
                        return users.OrderByDescending(u => u.Email).ToList();
                case "UserGroup.Name":
                    if (isAsc)
                    {
                        var usersList = users.OrderBy(u => Translation.Get(u.UserGroup.Name, Enums.TranslationSource.TextTranslation)).ToList();
                        return usersList;
                    }
                    else
                    {
                        var usersList = users.OrderByDescending(u => Translation.Get(u.UserGroup.Name, Enums.TranslationSource.TextTranslation)).ToList();
                        return usersList;
                    }
                case "ChangeTime":
                    if (isAsc)
                        return users.OrderBy(u => u.ChangeTime).ToList();
                    else
                        return users.OrderByDescending(u => u.ChangeTime).ToList();

                default: return users;
            }
        }
        /// <summary>
        /// The new.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [CustomAuthorize(Roles = "3,4")]
        public ActionResult New()
        {
            var filter = (UserSearch)this.Session["UserSearch"];
            if (filter == null)
            {
                filter = new UserSearch { CustomerId = SessionFacade.CurrentCustomer.Id };
            }

            var model = this.CreateInputViewModel(new User
            {
                ActivateCasePermission = 1,
                BulletinBoardDate = DateTime.Now,
                ChangeTime = DateTime.Now,
                CloseCasePermission = 1,
                CopyCasePermission = 1,
                DeleteCasePermission = 1,
                FAQPermission = 1,
                IsActive = 1,
                Language_Id = SessionFacade.CurrentLanguageId,
                MoveCasePermission = 1,
                PasswordChangedDate = DateTime.Now,
                Performer = 1,
                RefreshContent = 300,
                RegTime = DateTime.Now,
                ReportPermission = 1,
                SessionTimeout = 120,
                SetPriorityPermission = 1,
                ShowNotAssignedWorkingGroups = 1,
                StartPage = 1,
                Customer_Id = 0,
                UserGroup_Id = 4,
                TimeZoneId = SessionFacade.CurrentUser.TimeZoneId ?? TimeZoneInfo.Local.Id,
                CaseInternalLogPermission = 1
            });


            return this.View(model);
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        public ActionResult New(
            UserInputViewModel userInputViewModel, 
            int[] AAsSelected, 
            int[] CsSelected, 
            int[] OTsSelected, 
            string NewPassword, 
            string ConfirmPassword, 
            FormCollection coll)
        {
            IDictionary<string, string> errors;
            var user = this.returnCaseInfoMailForNewSave(userInputViewModel);
            userInputViewModel.User.ActivateCasePermission = 1;
            userInputViewModel.User.BulletinBoardDate = DateTime.Now;
            userInputViewModel.User.ChangeTime = DateTime.Now;
            userInputViewModel.User.CloseCasePermission = 1;
            userInputViewModel.User.CopyCasePermission = 1;
            userInputViewModel.User.DeleteCasePermission = 1;
            if (userInputViewModel.User.UserGroup_Id != (int)UserGroup.Administrator)
            {
                userInputViewModel.User.FAQPermission = 1;
            }

            userInputViewModel.User.IsActive = 1;
            userInputViewModel.User.MoveCasePermission = 1;
            userInputViewModel.User.Password = NewPassword;
            userInputViewModel.User.PasswordChangedDate = DateTime.Now;
            userInputViewModel.User.Performer = 1;
            userInputViewModel.User.RegTime = DateTime.Now;
            userInputViewModel.User.ReportPermission = 1;
            userInputViewModel.User.SetPriorityPermission = 1;
            if (userInputViewModel.User.UserGroup_Id == (int)UserGroup.User)
            {
                userInputViewModel.User.Performer = 0;
                userInputViewModel.User.CloseCasePermission = 0;
                userInputViewModel.User.CopyCasePermission = 0;
                userInputViewModel.User.DeleteCasePermission = 0;
                userInputViewModel.User.FAQPermission = 0;
                userInputViewModel.User.MoveCasePermission = 0;
                userInputViewModel.User.ReportPermission = 0;
                userInputViewModel.User.SetPriorityPermission = 0;
                userInputViewModel.User.ActivateCasePermission = 0;
            }

            //TimezoneId
            user.TimeZoneId = userInputViewModel.SelectedTimeZone;

            // validating available projects
            if (SessionFacade.CurrentUser.UserGroupId != (int)UserGroup.SystemAdministrator)
            {
                var availableCustomersHash = this._userService.GetUser(SessionFacade.CurrentUser.Id).CusomersAvailable.ToDictionary(it => it.Id, it => true);
                CsSelected = CsSelected.Where(availableCustomersHash.ContainsKey).ToArray();
            }

            if (user.UserRoles != null)
                foreach (var delete in user.UserRoles.ToList())
                    user.UserRoles.Remove(delete);
            else
                user.UserRoles = new List<UserRole>();

            if (userInputViewModel.UserRights.HasValue)
            {
                var userRight = this._userService.GetUserRoleById(userInputViewModel.UserRights.Value);
                user.UserRoles.Add(userRight);
            }

            this._userService.SaveNewUser(user, AAsSelected, CsSelected, OTsSelected, null, null, out errors, ConfirmPassword);
            if (errors.Count == 0)
            {
                return this.RedirectToAction("edit", "users", new { id = user.Id });
            }

            var model = this.CreateInputViewModel(userInputViewModel.User);
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, Translation.Get(error.Value));
            }

            return this.View(model);
        }

        [CustomAuthorize(Roles = "3,4")]
        public ActionResult Edit(int id, int? copy)
        {
            var user = copy.ToBool() ? _userService.GetUserForCopy(id) : _userService.GetUser(id);

            if (user == null)
                return new HttpNotFoundResult("No user found...");
            
            var model = this.CreateInputViewModel(user);

            if (copy.ToBool())
            {
                model.User.Id = -1;
                model.User.UserID = string.Empty;
                model.User.FirstName = string.Empty;
                model.User.SurName = string.Empty;
                model.User.Address = string.Empty;
                model.User.PostalAddress = string.Empty;
                model.User.PostalCode = string.Empty;
                model.User.Phone = string.Empty;
                model.User.CellPhone = string.Empty;
                model.User.Email = string.Empty;
                model.User.Password = string.Empty;
                model.CopyUserid = id;
            }

            return this.View(model);
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        public ActionResult Edit(
            int id, 
            int[] AAsSelected,
            int[] CsSelected, 
            int[] OTsSelected, 
            int[] Departments, 
            List<UserWorkingGroup> UserWorkingGroups, 
            UserSaveViewModel userModel, 
            string NewPassword, 
            string ConfirmPassword, 
            FormCollection coll)
        {
            IDictionary<string, string> errors;

            var userToSave = new User();

            if (id != -1)
            {
                userToSave = this._userService.GetUser(id);

                var currentUserData = this._userService.GetUser(id);

                if (SessionFacade.CurrentUser.UserGroupId != (int)UserGroup.SystemAdministrator
                    && (SessionFacade.CurrentUser.UserGroupId < userToSave.UserGroup_Id || userModel.User.UserGroup_Id > SessionFacade.CurrentUser.UserGroupId))
                {
                    return this.RedirectToAction("Forbidden", "Error", new { area = string.Empty });
                }

                userModel.User.CustomerUsers = currentUserData.CustomerUsers;

                userToSave.OrderPermission = this.returnOrderPermissionForSave(userModel);
                userToSave.CaseInfoMail = this.returnCaseInfoMailForEditSave(userModel);
                userToSave.TimeZoneId = userModel.SelectedTimeZone;

                this.TryUpdateModel(userToSave, "user");
                var allCustomers = _customerService.GetAllCustomers();
                string err = "";
                List<string> customersAlert = new List<string>(); 
                if (userToSave.IsActive == 0)
                {
                    var emptyWG = new List<int>();
                    foreach (var c in allCustomers)
                    {                        
                        if (_userService.UserHasActiveCase(c.Id, userToSave.Id, emptyWG))
                             customersAlert.Add(c.Name);
                    }                                       
                }

                if (customersAlert.Any())
                {
                    err = Translation.GetCoreTextTranslation("Användare") + " [" + userToSave.FirstName + " " + userToSave.SurName + "] " +
                          Translation.GetCoreTextTranslation("har aktiva ärenden hos kund") + ":";
                    err += "(" + string.Join(",", customersAlert.ToArray()) + ")|";
                    err += " " + Translation.GetCoreTextTranslation("För att se över dessa ärenden, gå till") + ":|";
                    err += " " + Translation.GetCoreTextTranslation("Ärendeöversikt") + "|";
                    err += " " + Translation.GetCoreTextTranslation("Pågående ärenden") + "|";
                    err += " " + Translation.GetCoreTextTranslation("Handläggare");
                }

                if (userToSave.UserRoles != null)
                {
                    foreach (var delete in userToSave.UserRoles.ToList())
                    {
                        userToSave.UserRoles.Remove(delete);
                    }

                    if (userModel.UserRights.HasValue)
                    {
                        var userRight = this._userService.GetUserRoleById(userModel.UserRights.Value);
                        userToSave.UserRoles.Add(userRight);
                    }
                }

                var customersAvailableHash = this.GetAvaliableCustomersFor(userToSave).ToDictionary(it => it.Id, it => true);
                
                int[] customersSelected = null;
                if (CsSelected != null)
                {
                    customersSelected = CsSelected.Where(customersAvailableHash.ContainsKey).ToArray();
                }
                

                if (SessionFacade.CurrentUser.Id == userToSave.Id)
                {
                    this._userService.SaveEditUser(
                       userToSave,
                       AAsSelected,
                       customersSelected,
                       customersAvailableHash.Keys.ToArray(),
                       OTsSelected,
                       Departments,
                       UserWorkingGroups,
                       out errors);
                }
                else
                {
                    // when user updates info about other user it is possible that second user has own selected customers that first user does not have
                    var usersOwnCustomer =
                        userToSave.CusomersAvailable.Where(it => !customersAvailableHash.ContainsKey(it.Id))
                            .Select(it => it.Id)
                            .ToList();
                    if (customersSelected != null)
                    {
                        usersOwnCustomer.AddRange(customersSelected);
                    }
                    this._userService.SaveEditUser(
                        userToSave,
                        AAsSelected,
                        customersSelected,
                        usersOwnCustomer.ToArray(),
                        OTsSelected,
                        Departments,
                        UserWorkingGroups,
                        out errors);
                }
                

                if (errors.Count == 0)
                {
                    if (!string.IsNullOrEmpty(err))
                    {
                        this.TempData["AlertMessage"] = err;
                    }
                    
                    return this.RedirectToAction("edit", "users", new { id = id });
                }
                
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Key, Translation.GetCoreTextTranslation(error.Value));
                }
                
                var model = this.CreateInputViewModel(userToSave);

                return this.View(model);
            }

            var copy = userModel.User;
            copy.Id = 0;
            copy.Password = NewPassword;
            copy.TimeZoneId = userModel.SelectedTimeZone;

            if (copy.Language_Id == 0)
            {
                copy.Language_Id = 2;
            }

            var tempCustomerUser = new List<CustomerUser>();
            if (copy.CustomerUsers != null)
            {
                tempCustomerUser = copy.CustomerUsers.ToList();
                copy.CustomerUsers.Clear();
            }

            if (userModel.UserRights.HasValue)
            {
                var userRight = this._userService.GetUserRoleById(userModel.UserRights.Value);
                if (copy.UserRoles == null)
                {
                    copy.UserRoles = new List<UserRole>();
                }

                copy.UserRoles.Add(userRight);
            }
  
            this._userService.SaveNewUser(copy, AAsSelected, CsSelected, OTsSelected, UserWorkingGroups, Departments, out errors, ConfirmPassword);

            if (errors.Count > 0)
            {
                copy.Id = -1;
                 var cmodel = this.CreateInputViewModel(copy);
            }
              

            var customerUsers = this._userService.GetCustomerUserForUser(copy.Id).ToList();

            foreach (var cu in customerUsers)
            {
                var ccu = tempCustomerUser.FirstOrDefault(x => x.Customer_Id == cu.Customer_Id);

                if (ccu == null)
                {
                    continue;
                }

                if (ccu.CasePriorityFilter != null)
                {
                    cu.CasePriorityFilter = "0";
                }

                if (ccu.CaseCaseTypeFilter != null)
                {
                    cu.CaseCaseTypeFilter = "0";
                }

                if (ccu.CaseCategoryFilter != null)
                {
                    cu.CaseCategoryFilter = "0";
                }

                if (ccu.CaseProductAreaFilter != null)
                {
                    cu.CaseProductAreaFilter = "0";
                }

                if (ccu.CaseRegionFilter != null)
                {
                    cu.CaseRegionFilter = "0";
                }

                if (ccu.CaseResponsibleFilter != null)
                {
                    cu.CaseResponsibleFilter = "0";
                }

                if (ccu.CaseStateSecondaryFilter != null)
                {
                    cu.CaseStateSecondaryFilter = "0";
                }

                if (ccu.CaseDepartmentFilter != null)
                {
                    cu.CaseDepartmentFilter = "0";
                }

                if (ccu.CaseStatusFilter != null)
                {
                    cu.CaseStatusFilter = "0";
                }

                if (ccu.CaseUserFilter != null)
                {
                    cu.CaseUserFilter = "0";
                }

                if (ccu.CaseWorkingGroupFilter != null)
                {
                    cu.CaseWorkingGroupFilter = "0";
                }
               
                cu.CasePerformerFilter = "0";

                cu.PriorityPermission = ccu.PriorityPermission;
                cu.CaptionPermission = ccu.CaptionPermission;
                cu.ContactBeforeActionPermission = ccu.ContactBeforeActionPermission;
                cu.StateSecondaryPermission = ccu.StateSecondaryPermission;
                cu.WatchDatePermission = ccu.WatchDatePermission;

                this._customerUserService.SaveCustomerUser(cu, out errors);

                var caseSettingsToCopy = new List<CaseSettings>();
                if (userModel != null && userToSave != null)
                {
                    this._caseSettingsService.GetCaseSettingsWithUser(cu.Customer_Id, userModel.CopyUserid, userToSave.UserGroup_Id);                    
                }    

                if (caseSettingsToCopy != null)
                {
                    foreach (var cs in caseSettingsToCopy)
                    {
                        var newUserCaseSetting = new CaseSettings();

                        newUserCaseSetting.User_Id = copy.Id;
                        newUserCaseSetting.Customer_Id = cs.Customer_Id;
                        newUserCaseSetting.Name = cs.Name;
                        newUserCaseSetting.Line = cs.Line;
                        newUserCaseSetting.MinWidth = cs.MinWidth;
                        newUserCaseSetting.UserGroup = cs.UserGroup;
                        newUserCaseSetting.ColOrder = cs.ColOrder;

                        this._caseSettingsService.SaveCaseSetting(newUserCaseSetting, out errors);
                    }                    
                }
            }

            if (errors.Count == 0)
            {
               // return this.RedirectToAction("edit", "users", new { id = id });
                return this.RedirectToAction("index", "users");
            }
            else
            {
                TempData["PreventError"] = errors.FirstOrDefault().Value;
            }

            var copyModel = this.CreateInputViewModel(copy);

            return this.View(copyModel);
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var allCustomers = _customerService.GetAllCustomers();
            string err = "";
            List<string> customersAlert = new List<string>();
            
            var emptyWG = new List<int>();
            foreach (var c in allCustomers)
            {
                if (_userService.UserHasActiveCase(c.Id, id, emptyWG))
                    customersAlert.Add(c.Name);
            }
                        
            if (customersAlert.Any())
            {
                err = Translation.Get("Det är inte möjligt att ta bort denna användare. Var vänlig inaktivera användaren istället") + ": ";
                err += "(" + string.Join(",", customersAlert.ToArray()) + ") ";
                TempData["PreventError"] = err;                
                return this.RedirectToAction("edit", "users", new { id = id });
            }                        

            if (this._userService.DeleteUser(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "users");
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "users", new { id = id });
            }
        }

        [HttpGet]
        public PartialViewResult UnlockCase(int caseId, int? selectedCustomerId = null, decimal caseNumber = 0, string searchText = "")
        {
            _caseLockService.UnlockCaseByCaseId(caseId);
            var model = GetLockedCaseModel(selectedCustomerId, caseNumber, searchText);
            return PartialView("Index/_LockedCaseOverview", model);
        }

        [HttpGet]
        public PartialViewResult GetLoggedInUsers(int? selectedCustomerId = null)
        {            
            var model = new List<LoggedInUsers>();
            SessionFacade.AdminUsersPageLoggedInUsersTabSelectedCustomerId = selectedCustomerId;
            if (selectedCustomerId.HasValue)
            {
                model = ApplicationFacade.GetLoggedInUsers(selectedCustomerId.Value).ToList();
            }
            else
            {
                model = ApplicationFacade.GetAllLoggedInUsers().ToList();
            }

            return PartialView("Index/_OnlineUserList", model);
        }

        [HttpGet]
        public PartialViewResult FilterLockedCases(int? selectedCustomerId = null, decimal caseNumber = 0, string searchText = "")
        {            
            var model = GetLockedCaseModel(selectedCustomerId, caseNumber, searchText);
            SessionFacade.AdminUsersPageLockedCasesTabSelectedCustomerId = selectedCustomerId;
            return PartialView("Index/_LockedCaseTable", model.LockedCasesModels);

        }
        private UserIndexViewModel IndexInputViewModel()
        {
            var user = this._userService.GetUser(SessionFacade.CurrentUser.Id);
            var csSelected = user.Cs ?? new List<Customer>();
            var csAvailable = new List<Customer>();

            foreach (var c in this._customerService.GetAllCustomers())
            {
                if (!csSelected.Contains(c))
                    csAvailable.Add(c);
            }

            List<SelectListItem> sli = new List<SelectListItem>();
            sli.Add(new SelectListItem()
            {
                Text = Translation.Get("Aktiva", Enums.TranslationSource.TextTranslation),
                Value = "1",
                Selected = false
            });
            sli.Add(new SelectListItem()
            {
                Text = Translation.Get("Inaktiva", Enums.TranslationSource.TextTranslation),
                Value = "2",
                Selected = false
            });
            sli.Add(new SelectListItem()
            {
                Text = Translation.Get("Alla", Enums.TranslationSource.TextTranslation),
                Value = "3",
                Selected = false
            });
            
            //Locked Cases                        
            int AdminUsersPageLockedCasesTabSelectedCustomerId = 0;
            if (!SessionFacade.AdminUsersPageLockedCasesTabSelectedCustomerId.HasValue)
            {
                SessionFacade.AdminUsersPageLockedCasesTabSelectedCustomerId = SessionFacade.CurrentUser.CustomerId;
            }

            AdminUsersPageLockedCasesTabSelectedCustomerId = SessionFacade.AdminUsersPageLockedCasesTabSelectedCustomerId.Value;
            var lockedCasesModel = this.GetLockedCaseModel(AdminUsersPageLockedCasesTabSelectedCustomerId);

            int AdminUsersPageLoggedInUsersTabSelectedCustomerId = 0;
            if (!SessionFacade.AdminUsersPageLoggedInUsersTabSelectedCustomerId.HasValue)
            {
                SessionFacade.AdminUsersPageLoggedInUsersTabSelectedCustomerId = SessionFacade.CurrentUser.CustomerId;
            }

            AdminUsersPageLoggedInUsersTabSelectedCustomerId = SessionFacade.AdminUsersPageLoggedInUsersTabSelectedCustomerId.Value;
            var model = new UserIndexViewModel
            {
                User = user,
                StatusUsers = sli,
                LockedCaseModel = lockedCasesModel,
                ListLoggedInUsers = AdminUsersPageLoggedInUsersTabSelectedCustomerId == 0 ? ApplicationFacade.GetAllLoggedInUsers() : ApplicationFacade.GetLoggedInUsers(AdminUsersPageLoggedInUsersTabSelectedCustomerId),
                Filter = new UserSearch { CustomerId = AdminUsersPageLockedCasesTabSelectedCustomerId },
                CsSelected = csSelected.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),                    
                })
                .OrderBy(c => c.Text)
                .ToList(),
                OnlineUsersTabSelectedCustomerId = AdminUsersPageLoggedInUsersTabSelectedCustomerId,
                UserOverviewList = new UserList()
            };
            
            return model;
        }

        private LockedCaseOverviewModel GetLockedCaseModel(int? selectedCustomerId = null, decimal caseNumber = 0, string searchText = "")
        {
            var user = this._userService.GetUser(SessionFacade.CurrentUser.Id);

            var csSelected = user.Cs ?? new List<Customer>();
            var csAvailable = new List<Customer>();

            foreach (var c in this._customerService.GetAllCustomers())
            {
                if (!csSelected.Contains(c))
                    csAvailable.Add(c);
            }
          
            var customerList = csSelected.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = (selectedCustomerId != null && x.Id == selectedCustomerId.Value)
                })
                .OrderBy(c=> c.Text)
                .ToList();

            // Delete the empty choice added to the top of the customer List
            customerList.RemoveAt(0);

            var lockedCases = new List<LockedCaseOverview>();
            if (caseNumber > 0)
                lockedCases = this._caseLockService.GetLockedCases(selectedCustomerId, caseNumber);
            else
                if (searchText != string.Empty)
                    lockedCases = this._caseLockService.GetLockedCases(selectedCustomerId, searchText);
                else
                    lockedCases = this._caseLockService.GetLockedCases(selectedCustomerId);

            var lockedCaseModel = lockedCases.Select(l => l.MapToViewModel()).OrderBy(u=> u.UserId).ToList();
            var model = new LockedCaseOverviewModel(customerList, selectedCustomerId, caseNumber, searchText, lockedCaseModel);

            return model;
        }

        private UserInputViewModel CreateInputViewModel(User user)
        {
            #region Generals

            var aasSelected = user.AAs ?? new List<AccountActivity>();
            var aasAvailable = new List<AccountActivity>();
            foreach (var aa in this._accountActivityService.GetAccountActivities(SessionFacade.CurrentCustomer.Id))
            {
                if (!aasSelected.Contains(aa))
                    aasAvailable.Add(aa);
            }
           
            var customersSelected = user.Cs ?? new List<Customer>();
            var selectedCustomersHash = customersSelected.ToDictionary(it => it.Id, it => true);
            var customersAvailable = this.GetAvaliableCustomersFor(user)
                .Where(it => !selectedCustomersHash.ContainsKey(it.Id))
                .OrderBy(it => it.Name);
            var otsSelected = user.OTs ?? new List<OrderType>();
            var otsAvailable = new List<OrderType>();
            
            foreach (var ot in this._orderTypeService.GetOrderTypes(SessionFacade.CurrentCustomer.Id))
            {
                if (!otsSelected.Contains(ot))
                    otsAvailable.Add(ot);
            }

            #endregion

            #region SelectListItems

            var lis = new List<SelectListItem>();
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("Start"),
                Value = ((int)StartPage.Start).ToString(CultureInfo.InvariantCulture),
                Selected = false
            });
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("Ärendeöversikt"),
                Value = ((int)StartPage.CaseSummary).ToString(CultureInfo.InvariantCulture),
                Selected = false
            });

            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Varje vecka", Enums.TranslationSource.TextTranslation),
                Value = "1",
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Varje dag", Enums.TranslationSource.TextTranslation),
                Value = "127",
                Selected = false
            });

            List<SelectListItem> sli = new List<SelectListItem>();
            sli.Add(new SelectListItem()
            {
                Text = "",
                Value = "0",
                Selected = false
            });
            sli.Add(new SelectListItem()
            {
                Text = "1",
                Value = "60",
                Selected = false
            });
            sli.Add(new SelectListItem()
            {
                Text = "2",
                Value = "120",
                Selected = false
            });
            sli.Add(new SelectListItem()
            {
                Text = "3",
                Value = "180",
                Selected = false
            });
            sli.Add(new SelectListItem()
            {
                Text = "4",
                Value = "240",
                Selected = false
            });
            sli.Add(new SelectListItem()
            {
                Text = "5",
                Value = "300",
                Selected = false
            });
            sli.Add(new SelectListItem()
            {
                Text = "10",
                Value = "600",
                Selected = false
            });
            sli.Add(new SelectListItem()
            {
                Text = "15",
                Value = "900",
                Selected = false
            });

            #endregion

            #region Model

            var customerUsers = user.CustomerUsers ?? this._userService.GetCustomerUserForUser(user.Id);

            var model = new UserInputViewModel
            {
                User = user,
                CaseInfoMailList = li,
                RefreshInterval = sli,
                StartPageShowList = lis,
                CustomerUsers = customerUsers.Where(x => x.Customer_Id > 0).ToList(),
                Departments = this._userService.GetDepartmentsForUser(user.Id),
                ListWorkingGroupsForUser = this._userService.GetListToUserWorkingGroup(user.Id),
                AvailvableTimeZones = TimeZoneInfo.GetSystemTimeZones().Select(it => new SelectListItem() { Value = it.Id, Text = it.DisplayName, Selected = user.TimeZoneId == it.Id }),
                Customers = this._customerService.GetAllCustomers().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Domains = this._domainService.GetDomains(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Name),
                    Value = x.Id.ToString()
                }).ToList(),
                UserRoles = this._userService.GetUserRoles().Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Description, Enums.TranslationSource.TextTranslation),
                    Value = x.Id.ToString(),
                }).ToList(),

                AAsAvailable = aasAvailable.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                AAsSelected = aasSelected.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                CsAvailable = customersAvailable.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                CsSelected = customersSelected.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).OrderBy(it => it.Text).ToList(),
                OTsAvailable = otsAvailable.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                OTsSelected = otsSelected.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                WorkingGroups = this._workingGroupService.GetAllWorkingGroups().Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList()
            };

            model.UserGroups = this._userService.GetUserGroups()
                .Where(it => it.Id <= SessionFacade.CurrentUser.UserGroupId)
                .OrderBy(x => x.Id)
                .Select(x => new SelectListItem
            {
                Text = Translation.Get(x.Name),
                Value = x.Id.ToString()
            }).ToList();

            #endregion

            #region SetInts           

            if (model.User.Id != 0 && model.User.Id != -1)
            {
                if (model.User.UserRoles.Any())
                    model.UserRights = model.User.UserRoles.FirstOrDefault().Id;
                else
                    model.UserRights = 0;
            }


            if (user.CaseInfoMail == 0)
            {
                model.SendMailYesNo = 0;
            }
            else if (user.CaseInfoMail == 1)
            {
                model.SendMailYesNo = 1;
                model.SendMailYesNoWhen = 1;
            }
            else if (user.CaseInfoMail == 127)
            {
                model.SendMailYesNo = 1;
                model.SendMailYesNoWhen = 127;
            }

            if (user.OrderPermission == 1)
            {
                model.OrderP1 = 1;
            }
            else if (user.OrderPermission == 2)
            {
                model.OrderP2 = 1;
            }
            else
            {
                model.OrderP1 = 0;
                model.OrderP2 = 0;
            }

            // Edit mode
            if (user.Id > 0)
            {
                var userCustomerSetting = this._settingService.GetCustomerSetting(user.Customer_Id);

                model.ChangePasswordModel = new ChangePasswordModel()
                {
                    UserId = user.Id,
                    MinPasswordLength = userCustomerSetting.MinPasswordLength,
                    UseComplexPassword = userCustomerSetting.ComplexPassword.ToBool()
                };
            }

            #endregion

            return model;
        }

        private IEnumerable<Customer> GetAvaliableCustomersFor(User user)
        {
            if (SessionFacade.CurrentUser.UserGroupId == (int)UserGroup.SystemAdministrator)
            {
                return this._customerService.GetAllCustomers().ToArray();
            }
            
            if (user.IsNew())
            {
                return this._userService.GetUser(SessionFacade.CurrentUser.Id).CusomersAvailable.OrderBy(it => it.Name).ToList();
            }

            if (SessionFacade.CurrentUser.Id == user.Id)
            {
                return user.CusomersAvailable.ToArray();
            }

            var combinedAvailableCustomers = this._userService.GetUser(SessionFacade.CurrentUser.Id).CusomersAvailable.ToDictionary(it => it.Id, it => it);
            foreach (var customer in user.CusomersAvailable.ToArray())
            {
                if (!combinedAvailableCustomers.ContainsKey(customer.Id))
                {
                    combinedAvailableCustomers.Add(customer.Id, customer);
                }
            }

            return combinedAvailableCustomers.Select(it => it.Value).ToArray();
        }

        private User returnCaseInfoMailForNewSave(UserInputViewModel userInputViewModel)
        {
            var user = userInputViewModel.User;

            if (userInputViewModel.SendMailYesNo == 0)
            {
                user.CaseInfoMail = 0;
            }
            else
            {
                if (userInputViewModel.SendMailYesNoWhen == 1)
                {
                    user.CaseInfoMail = 1;
                }
                else
                    user.CaseInfoMail = 127;
            }

            return user;
        }

        private int returnCaseInfoMailForEditSave(UserSaveViewModel userModel)
        {
            int sendMail = 0;

            if (userModel.SendMailYesNo == 0)
            {
                sendMail = 0;
            }
            else
            {
                if (userModel.SendMailYesNoWhen == 1)
                {
                    sendMail = 1;
                }
                else
                    sendMail = 127;
            }

            return sendMail;
        }

        private int returnOrderPermissionForSave(UserSaveViewModel userModel)
        {
            userModel.UserOrderPermission = 0;

            if (userModel.OrderP1 == 1 && userModel.OrderP2 == 1)
            {
                userModel.UserOrderPermission = 2;
            }
            else if (userModel.OrderP1 == 1)
            {
                userModel.UserOrderPermission = 1;
            }
            else if (userModel.OrderP2 == 1)
            {
                userModel.UserOrderPermission = 2;
            }
            else
                userModel.UserOrderPermission = 0;

            return userModel.UserOrderPermission;
        }

        private string NewUserPassword(int id, string newPassword, string confirmPassword)
        {
            var user = this._userService.GetUser(id);

            if (newPassword == confirmPassword)
            {
                user.Password = newPassword;
            }
            else
                throw new ArgumentNullException("The password fields do not  match, please re-type them...");

            return user.Password;
        }

        [HttpPost]
        public void EditUserPassword(int id, string newPassword, string confirmPassword)
        {
            if (newPassword == confirmPassword)
            {
                this._userService.SavePassword(id, newPassword);
            }
            else
                throw new ArgumentNullException("The password fields do not  match, please re-type them...");
        }
    }
}