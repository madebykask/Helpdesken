using DH.Helpdesk.BusinessData.Models.User;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.Boolean;
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
    using System.Threading.Tasks;

    public class UsersController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly ICustomerUserService _customerUserService;
        private readonly IDomainService _domainService;
        private readonly ILanguageService _languageService;
        private readonly IOrderTypeService _orderTypeService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ICaseSettingsService _caseSettingsService;
        private readonly ICaseLockService _caseLockService;
        private readonly ISettingService _settingService;
        private readonly IContractCategoryService _contractCategoryService;

        public UsersController(
            ICustomerService customerService,
            ICustomerUserService customerUserService,
            IDomainService domainService,
            ILanguageService languageService,
            IOrderTypeService orderTypeService,
            IUserService userService,
            IWorkingGroupService workingGroupService,
            ICaseSettingsService caseSettingsService,
            ICaseLockService caseLockService,
            IMasterDataService masterDataService,
            ISettingService settingService,
            IContractCategoryService contractCategoryService)
            : base(masterDataService)
        {
            this._customerService = customerService;
            this._customerUserService = customerUserService;
            this._domainService = domainService;
            this._languageService = languageService;
            this._orderTypeService = orderTypeService;
            this._userService = userService;
            this._workingGroupService = workingGroupService;
            this._caseSettingsService = caseSettingsService;
            this._caseLockService = caseLockService;
            this._settingService = settingService;
            this._contractCategoryService = contractCategoryService;
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

            if (this.Session["UserSearch"] == null)
            {
                // TODO: add all customers support if "All Customers selected by default"
                model.UserOverviewList.Users = SessionFacade.CurrentUser.UserGroupId == UserGroups.CustomerAdministrator ?
                    this._userService.GetUsersByUserGroup(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.UserID).ToList() :
                    this._userService.GetUsers(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.UserID).ToList();
                model.UserOverviewList.Sorting = new UserSort { FieldName = "", IsAsc = true };
            }
            else
            {
                var filter = (UserSearch)this.Session["UserSearch"];
                model.Filter = filter;
                model.UserOverviewList.Users = this._userService.SearchSortAndGenerateUsers(filter, new List<int> { filter.CustomerId },
                    SessionFacade.CurrentUser.UserGroupId == UserGroups.CustomerAdministrator);

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
            var userSearch = (UserSearch)this.Session["UserSearch"];

            if (this.Session["UserSearch"] == null && filter.SearchUs == null)
            {
                model.UserOverviewList.Users = this._userService.GetUsers(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.UserID).ToList();
                model.UserOverviewList.Sorting = new UserSort { FieldName = "", IsAsc = true };
            }
            var customersIds = filter.CustomerId == -1
                ? model.CsSelected.Select(c => int.Parse(c.Value))
                : new[] {filter.CustomerId};

            model.UserOverviewList.Users = this._userService.SearchSortAndGenerateUsers(filter, customersIds.ToList(),
                SessionFacade.CurrentUser.UserGroupId == UserGroups.CustomerAdministrator);

            if (this.Session["UserSearch"] == null && filter.SearchUs != null)
            {
                model.UserOverviewList.Sorting = new UserSort { FieldName = "", IsAsc = true };
            }
            else
            {
                model.Filter = filter;
                this.Session["UserSearch"] = filter;
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
                model.Users = this._userService.GetAllUsers().OrderBy(x => x.UserID).ToList();
                model.Sorting = new UserSort { FieldName = fieldName, IsAsc = !isAsc };
            }
            var user = this._userService.GetUser(SessionFacade.CurrentUser.Id);
            var csSelected = user.Cs.Where(c => c.Status == 1).ToList();

            var customersIds = filter.CustomerId == -1
                ? csSelected.Select(c => c.Id)
                : new[] {filter.CustomerId};
            model.Users = this._userService.SearchSortAndGenerateUsers(filter, customersIds.ToList(),
                SessionFacade.CurrentUser.UserGroupId == UserGroups.CustomerAdministrator);
            if (this.Session["UserSearch"] == null && filter.SearchUs != null)
            {
                model.Sorting = new UserSort { FieldName = fieldName, IsAsc = !isAsc };
            }

            model.Users = this.SortUsers(model.Users, fieldName, isAsc);
            model.Sorting = new UserSort { FieldName = fieldName, IsAsc = !isAsc };

            return PartialView("Index/_UserGrid", model);
        }

        private IEnumerable<User> SortUsers(IEnumerable<User> users, string fieldName, bool isAsc)
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
            var user = new User
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
            };

            var model = this.CreateInputViewModel(user, user.Id);


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

            var model = this.CreateInputViewModel(userInputViewModel.User, userInputViewModel.User.Id);
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, Translation.GetCoreTextTranslation(error.Value));
            }

            return this.View(model);
        }

        [CustomAuthorize(Roles = "3,4")]
        public ActionResult Edit(int id, int? copy)
        {
            var user = copy.ToBool() ? _userService.GetUserForCopy(id) : _userService.GetUser(id);

            if (user == null)
                return new HttpNotFoundResult("No user found...");

            var model = this.CreateInputViewModel(user, user.Id);

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
        public ActionResult Edit(int id, UserSaveInputModel inputModel)
        {
            IDictionary<string, string> errors;

            User userToSave;
            var isCopy = id == -1;

            if (!isCopy)
            {
                userToSave = _userService.GetUser(id);

                if (SessionFacade.CurrentUser.UserGroupId != (int)UserGroup.SystemAdministrator &&
                    (SessionFacade.CurrentUser.UserGroupId < userToSave.UserGroup_Id ||
                     inputModel.User.UserGroup_Id > SessionFacade.CurrentUser.UserGroupId))
                {
                    {
                        return RedirectToAction("Forbidden", "Error", new { area = string.Empty });
                    }
                }

                errors = SaveUserEdit(inputModel, userToSave);

                if (errors.Count == 0)
                    return RedirectToAction("edit", "users", new { id = id });
            }
            else
            {
                userToSave = inputModel.User; //copy
                errors = SaveUserNew(inputModel, userToSave);
            }

            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Key, Translation.GetCoreTextTranslation(error.Value));
                }

                var userId = userToSave.Id;
                if (isCopy) userId = inputModel.CopyUserId;

                var model = CreateInputViewModel(userToSave, userId);
                return View(model);
            }

            return RedirectToAction("index", "users");
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var allCustomers = _customerService.GetAllCustomers();
            var customersAlert = new List<string>();

            var emptyWG = new List<int>();
            foreach (var c in allCustomers)
            {
                if (_userService.UserHasActiveCase(c.Id, id, emptyWG))
                    customersAlert.Add(c.Name);
            }

            if (customersAlert.Any())
            {
                var err = Translation.GetCoreTextTranslation("Det är inte möjligt att ta bort denna användare. Var vänlig inaktivera användaren istället") + ": ";
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
            if (selectedCustomerId.HasValue && selectedCustomerId.Value != -1)
                model = ApplicationFacade.GetLoggedInUsers(selectedCustomerId.Value).ToList();
            else
                model = ApplicationFacade.GetAllLoggedInUsers().ToList();

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
            var csSelected = user.Cs.Where(c => c.Status == 1).ToList();

            var sli = new List<SelectListItem>();
            sli.Add(new SelectListItem()
            {
                Text = Translation.Get("Aktiva"),
                Value = "1",
                Selected = false
            });
            sli.Add(new SelectListItem()
            {
                Text = Translation.Get("Inaktiva"),
                Value = "2",
                Selected = false
            });
            sli.Add(new SelectListItem()
            {
                Text = Translation.Get("Alla"),
                Value = "3",
                Selected = false
            });

            //Locked Cases                        
            var adminUsersPageLockedCasesTabSelectedCustomerId = 0;
            if (!SessionFacade.AdminUsersPageLockedCasesTabSelectedCustomerId.HasValue)
            {
                SessionFacade.AdminUsersPageLockedCasesTabSelectedCustomerId = SessionFacade.CurrentUser.CustomerId;
            }

            adminUsersPageLockedCasesTabSelectedCustomerId = SessionFacade.AdminUsersPageLockedCasesTabSelectedCustomerId.Value;
            var lockedCasesModel = this.GetLockedCaseModel(adminUsersPageLockedCasesTabSelectedCustomerId);

            var adminUsersPageLoggedInUsersTabSelectedCustomerId = 0;
            if (!SessionFacade.AdminUsersPageLoggedInUsersTabSelectedCustomerId.HasValue)
            {
                SessionFacade.AdminUsersPageLoggedInUsersTabSelectedCustomerId = SessionFacade.CurrentUser.CustomerId;
            }

            adminUsersPageLoggedInUsersTabSelectedCustomerId = SessionFacade.AdminUsersPageLoggedInUsersTabSelectedCustomerId.Value;
            var model = new UserIndexViewModel
            {
                User = user,
                StatusUsers = sli,
                LockedCaseModel = lockedCasesModel,
                ListLoggedInUsers = adminUsersPageLoggedInUsersTabSelectedCustomerId == 0 ?
                    ApplicationFacade.GetAllLoggedInUsers() :
                    ApplicationFacade.GetLoggedInUsers(adminUsersPageLoggedInUsersTabSelectedCustomerId),
                Filter = new UserSearch { CustomerId = adminUsersPageLockedCasesTabSelectedCustomerId },
                CsSelected = csSelected.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                })
                .OrderBy(c => c.Text)
                .ToList(),
                OnlineUsersTabSelectedCustomerId = adminUsersPageLoggedInUsersTabSelectedCustomerId,
                UserOverviewList = new UserList()
            };

            if (SessionFacade.CurrentUser.UserGroupId == UserGroups.SystemAdministrator)
            {
                model.CsSelected.Insert(0, new SelectListItem
                {
                    Text = Translation.Get("Alla kunder"),
                    Value = "-1",
                });
            }

            return model;
        }

        private LockedCaseOverviewModel GetLockedCaseModel(int? selectedCustomerId = null, decimal caseNumber = 0, string searchText = "")
        {
            var user = this._userService.GetUser(SessionFacade.CurrentUser.Id);

            var csSelected = user.Cs ?? new List<Customer>();

            var customerList = csSelected.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = (selectedCustomerId != null && x.Id == selectedCustomerId.Value)
            })
                .OrderBy(c => c.Text)
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

            var lockedCaseModel = lockedCases.Select(l => l.MapToViewModel()).OrderBy(u => u.UserId).ToList();
            var model = new LockedCaseOverviewModel(customerList, selectedCustomerId, caseNumber, searchText, lockedCaseModel);

            return model;
        }

        private UserInputViewModel CreateInputViewModel(User user, int userId)
        {
            #region Generals

            // not used 
            //var aasSelected = user.AAs ?? new List<AccountActivity>();
            //var aasAvailable = new List<AccountActivity>();
            //foreach (var aa in this._accountActivityService.GetAccountActivities(SessionFacade.CurrentCustomer.Id))
            //{
            //    if (!aasSelected.Contains(aa))
            //        aasAvailable.Add(aa);
            //}

            var customersSelected = user.Cs ?? new List<Customer>();
            customersSelected = customersSelected.Where(x => x.Status ==1 ).ToList();

            var selectedCustomersHash = customersSelected.ToDictionary(it => it.Id, it => true);
            var customersAvailable = GetAvaliableCustomersFor(user).Where(it => !selectedCustomersHash.ContainsKey(it.Id) && it.Status == 1).OrderBy(it => it.Name);
            var otsSelected = user.OTs ?? new List<OrderType>();
            var otsAvailable = new List<OrderType>();

            foreach (var ot in _orderTypeService.GetOrderTypes(SessionFacade.CurrentCustomer.Id))
            {
                if (!otsSelected.Contains(ot))
                    otsAvailable.Add(ot);
            }
            var ccsSelected = user.CCs ?? new List<ContractCategory>();
            var ccsAvailable = new List<ContractCategory>();

            foreach (var cc in _contractCategoryService.GetContractCategories(SessionFacade.CurrentCustomer.Id))
            {
                if (!ccsSelected.Contains(cc))
                    ccsAvailable.Add(cc);
            }
            #endregion

            #region SelectListItems

            var lis = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = Translation.GetCoreTextTranslation("Start"),
                    Value = ((int) StartPage.Start).ToString(CultureInfo.InvariantCulture),
                    Selected = false
                },
                new SelectListItem()
                {
                    Text = Translation.GetCoreTextTranslation("Ärendeöversikt"),
                    Value = ((int) StartPage.CaseSummary).ToString(CultureInfo.InvariantCulture),
                    Selected = false
                },
                new SelectListItem()
                {
                    Text = Translation.GetCoreTextTranslation("Avancerad sökning"),
                    Value = ((int) StartPage.AdvancedSearch).ToString(CultureInfo.InvariantCulture),
                    Selected = false
                }
            };

            var li = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = Translation.GetCoreTextTranslation("Varje vecka"), Value = "1", Selected = false
                },
                new SelectListItem()
                {
                    Text = Translation.GetCoreTextTranslation("Varje dag"), Value = "127", Selected = false
                }
            };

            var sli = new List<SelectListItem>
            {
                new SelectListItem() {Text = "", Value = "0", Selected = false},
                new SelectListItem() {Text = "1", Value = "60", Selected = false},
                new SelectListItem() {Text = "2", Value = "120", Selected = false},
                new SelectListItem() {Text = "3", Value = "180", Selected = false},
                new SelectListItem() {Text = "4", Value = "240", Selected = false},
                new SelectListItem() {Text = "5", Value = "300", Selected = false},
                new SelectListItem() {Text = "10", Value = "600", Selected = false},
                new SelectListItem() {Text = "15", Value = "900", Selected = false}
            };

            #endregion

            #region Model

            var customerUsers = (user.CustomerUsers?.Any() ?? false ? user.CustomerUsers : _userService.GetCustomerUserForUser(userId))
                                                                                           .Where(o => o.Customer.Status == 1).OrderBy(o => o.Customer.Name).ToList();

            var model = new UserInputViewModel
            {
                User = user,
                CaseInfoMailList = li,
                RefreshInterval = sli,
                StartPageShowList = lis,
                CustomerUsers = customerUsers.Where(x => x.Customer_Id > 0).Select(x => x.MapToCustomerUserEdit()).ToList(),
                Departments = _userService.GetDepartmentsForUser(userId),
                ListWorkingGroupsForUser = _userService.GetListToUserWorkingGroup(userId).Where(o => o.CustomerActive).ToList(),
                AvailvableTimeZones = TimeZoneInfo.GetSystemTimeZones().Select(it => new SelectListItem() { Value = it.Id, Text = it.DisplayName, Selected = user.TimeZoneId == it.Id }),

                Customers = _customerService.GetAllCustomers().Select(x => new StateSelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
					Active = x.Status == 1
					
                }).Where(o => o.Active).ToList(),

                Domains = _domainService.GetDomains(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                Languages = _languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = Translation.GetCoreTextTranslation(x.Name),
                    Value = x.Id.ToString()
                }).ToList(),

                UserRoles = _userService.GetUserRoles().Select(x => new SelectListItem
                {
                    Text = Translation.GetCoreTextTranslation(x.Description),
                    Value = x.Id.ToString(),
                }).ToList(),

                //AAsAvailable = aasAvailable.Select(x => new SelectListItem
                //{
                //    Text = x.Name,
                //    Value = x.Id.ToString()
                //}).ToList(),

                //AAsSelected = aasSelected.Select(x => new SelectListItem
                //{
                //    Text = x.Name,
                //    Value = x.Id.ToString()
                //}).ToList(),

                CsAvailable = customersAvailable.Select(x => new StateSelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
					Active = x.Status == 1
                }).ToList(),
                CsSelected = customersSelected.Select(x => new StateSelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
					Active = x.Status == 1
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
                CCsAvailable = ccsAvailable.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                CCsSelected = ccsSelected.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                WorkingGroups = this._workingGroupService.GetAllWorkingGroups().Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
                UserGroups = this._userService.GetUserGroups()
                    .Where(it => it.Id <= SessionFacade.CurrentUser.UserGroupId)
                    .OrderBy(x => x.Id)
                    .Select(x => new SelectListItem
                    {
                        Text = Translation.GetCoreTextTranslation(x.Name),
                        Value = x.Id.ToString()
                    }).ToList()

            };

            #endregion

            #region SetInts

            if (model.User.Id != 0 && model.User.Id != -1)
            {
                model.UserRights = model.User.UserRoles.Any() ? model.User.UserRoles.First().Id : 0;
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
            if (userId > 0)
            {
                var userCustomerSetting = this._settingService.GetCustomerSetting(user.Customer_Id);

                model.ChangePasswordModel = new ChangePasswordModel()
                {
                    UserId = userId,
                    MinPasswordLength = userCustomerSetting.MinPasswordLength,
                    UseComplexPassword = userCustomerSetting.ComplexPassword.ToBool()
                };
            }

            #endregion

            return model;
        }

        private IDictionary<string, string> SaveUserEdit(UserSaveInputModel inputModel, User userToSave)
        {
            IDictionary<string, string> errors;
            TryUpdateModel(userToSave, "user");

            userToSave.OrderPermission = returnOrderPermissionForSave(inputModel);
            userToSave.CaseInfoMail = returnCaseInfoMailForEditSave(inputModel);
            userToSave.TimeZoneId = inputModel.SelectedTimeZone;

            // Remove admin rights if no view right for inventory
            if (userToSave.InventoryViewPermission == 0)
            {
                userToSave.InventoryPermission = 0;
            }

            var allCustomers = _customerService.GetAllCustomers();
            var err = "";
            var customersAlert = new List<string>();

            if (userToSave.IsActive == 0)
            {
                var emptyWG = new List<int>();
                foreach (var c in allCustomers)
                {
                    if (_userService.UserHasActiveCase(c.Id, userToSave.Id, emptyWG))
                        customersAlert.Add(c.Name);
                }
            }

            //todo: move to other method
            if (customersAlert.Any())
            {
                err = Translation.GetCoreTextTranslation("Användare") + " [" + userToSave.FirstName + " " + userToSave.SurName +
                      "] " +
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

                if (inputModel.UserRights.HasValue)
                {
                    var userRight = _userService.GetUserRoleById(inputModel.UserRights.Value);
                    userToSave.UserRoles.Add(userRight);
                }
            }

            var customersAvailableHash = GetAvaliableCustomersFor(userToSave).ToDictionary(it => it.Id, it => true);

            int[] customersSelected = null;
            if (inputModel.CsSelected != null)
            {
                customersSelected = inputModel.CsSelected.Where(customersAvailableHash.ContainsKey).ToArray();
            }


            if (SessionFacade.CurrentUser.Id == userToSave.Id)
            {
                _userService.SaveEditUser(
                    userToSave,
                    inputModel.AAsSelected,
                    customersSelected,
                    customersAvailableHash.Keys.ToArray(),
                    inputModel.OTsSelected,
                    inputModel.CCsSelected,
                    inputModel.Departments,
                    inputModel.UserWorkingGroups,
                    inputModel.CustomerUsers,
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

                _userService.SaveEditUser(
                    userToSave,
                    inputModel.AAsSelected,
                    customersSelected,
                    usersOwnCustomer.ToArray(),
                    inputModel.OTsSelected,
                    inputModel.CCsSelected,
                    inputModel.Departments,
                    inputModel.UserWorkingGroups,
                    inputModel.CustomerUsers,
                    out errors);
            }

            if (errors.Count == 0 && !string.IsNullOrEmpty(err))
            {
                TempData["AlertMessage"] = err;
            }

            return errors;
        }

        private IDictionary<string, string> SaveUserNew(UserSaveInputModel inputModel, User userToSave)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            userToSave.Id = 0;
            userToSave.Password = inputModel.NewPassword;
            userToSave.TimeZoneId = inputModel.SelectedTimeZone;

            if (userToSave.Language_Id == 0)
            {
                userToSave.Language_Id = 2;
            }

            if (inputModel.UserRights.HasValue)
            {
                var userRight = _userService.GetUserRoleById(inputModel.UserRights.Value);
                if (userToSave.UserRoles == null)
                {
                    userToSave.UserRoles = new List<UserRole>();
                }

                userToSave.UserRoles.Add(userRight);
            }

            _userService.SaveNewUser(userToSave, inputModel.AAsSelected, inputModel.CsSelected, inputModel.OTsSelected,
                inputModel.UserWorkingGroups, inputModel.Departments, out errors, inputModel.ConfirmPassword);

            if (errors.Count > 0)
            {
                userToSave.Id = -1;
            }
            else
            {
                var customerUserChanges = GetCustomerUserChanges(inputModel.CopyUserId, inputModel.CustomerUsers);
                var newCustomerUsers = _userService.GetCustomerUserForUser(userToSave.Id).ToList();

                //Copy user customer settings after user with associated customers (tblCustomerUser) has been created
                foreach (var cu in newCustomerUsers)
                {
                    if (customerUserChanges != null && customerUserChanges.Any())
                    {
                        var ccu = customerUserChanges.FirstOrDefault(x => x.Customer_Id == cu.Customer_Id);
                        if (ccu == null)
                            continue;

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

                        //permissions
                        cu.UserInfoPermission = ccu.UserInfoPermission;
                        cu.CaptionPermission = ccu.CaptionPermission;
                        cu.ContactBeforeActionPermission = ccu.ContactBeforeActionPermission;
                        cu.PriorityPermission = ccu.PriorityPermission;
                        cu.StateSecondaryPermission = ccu.StateSecondaryPermission;
                        cu.WatchDatePermission = ccu.WatchDatePermission;
                        cu.RestrictedCasePermission = ccu.RestrictedCasePermission;

                        _customerUserService.SaveCustomerUser(cu);
                    }

                    var caseSettingsToCopy = _caseSettingsService
                        .GetCaseSettingsWithUser(cu.Customer_Id, inputModel.CopyUserId, userToSave.UserGroup_Id)
                        .ToList();

                    foreach (var cs in caseSettingsToCopy)
                    {
                        var newUserCaseSetting = new CaseSettings();

                        newUserCaseSetting.User_Id = userToSave.Id;
                        newUserCaseSetting.Customer_Id = cs.Customer_Id;
                        newUserCaseSetting.Name = cs.Name;
                        newUserCaseSetting.Line = cs.Line;
                        newUserCaseSetting.MinWidth = cs.MinWidth;
                        newUserCaseSetting.UserGroup = cs.UserGroup;
                        newUserCaseSetting.ColOrder = cs.ColOrder;

                        _caseSettingsService.SaveCaseSetting(newUserCaseSetting, out errors);
                    }
                }
            }

            return errors;
        }

        private IList<CustomerUser> GetCustomerUserChanges(int oldUserId, IList<CustomerUserForEdit> customerPermissions)
        {
            var customerUserSettingsCopy = _customerUserService.GetCustomerUsersForUserToCopy(oldUserId);

            foreach (var cusData in customerPermissions)
            {
                var cu = customerUserSettingsCopy.FirstOrDefault(x => x.Customer_Id == cusData.CustomerId);
                if (cu != null)
                {
                    cu.UserInfoPermission = cusData.UserInfoPermission.ToInt();
                    cu.CaptionPermission = cusData.CaptionPermission.ToInt();
                    cu.ContactBeforeActionPermission = cusData.ContactBeforeActionPermission.ToInt();
                    cu.PriorityPermission = cusData.PriorityPermission.ToInt();
                    cu.StateSecondaryPermission = cusData.StateSecondaryPermission.ToInt();
                    cu.WatchDatePermission = cusData.WatchDatePermission.ToInt();
                    cu.RestrictedCasePermission = cusData.RestrictedCasePermission;
                }
            }

            return customerUserSettingsCopy;
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

        private int returnCaseInfoMailForEditSave(UserSaveInputModel userModel)
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

        private int returnOrderPermissionForSave(UserSaveInputModel userModel)
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
        [HttpGet]
        public string GetEmailById(int id)
        {
            return _userService.GetUserEmail(id);
        }
    }
}