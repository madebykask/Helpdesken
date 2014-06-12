namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;
   

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
            IMasterDataService masterDataService)
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
                model.Users = this._userService.GetUsers(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.UserID).ToList();
            }
            else
            {
                var filter = (UserSearch)this.Session["UserSearch"];
                model.Filter = filter;
                model.Users = this._userService.SearchSortAndGenerateUsers(filter);
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
            model.Filter = filter;            
            this.Session["UserSearch"] = filter;
            model.Users = this._userService.SearchSortAndGenerateUsers(filter);

            return this.View(model);
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
                // MenuSettings = string.Empty,
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
                Customer_Id = filter.CustomerId,
                UserGroup_Id = 4
            });


            return this.View(model);
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        public ActionResult New(UserInputViewModel userInputViewModel, int[] AAsSelected, int[] CsSelected, int[] OTsSelected, string NewPassword, string ConfirmPassword, FormCollection coll)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            var user = this.returnCaseInfoMailForNewSave(userInputViewModel);

            //returnUserRoleForNewSave(userInputViewModel); TODO: Save userrole correct! geht nichts momental

            userInputViewModel.User.ActivateCasePermission = 1;
            userInputViewModel.User.BulletinBoardDate = DateTime.Now;
            userInputViewModel.User.ChangeTime = DateTime.Now;
            userInputViewModel.User.CloseCasePermission = 1;
            userInputViewModel.User.CopyCasePermission = 1;
            userInputViewModel.User.DeleteCasePermission = 1;
            userInputViewModel.User.FAQPermission = 1;
            userInputViewModel.User.IsActive = 1;
            userInputViewModel.User.MoveCasePermission = 1;
            //userInputViewModel.User.Password = this.NewUserPassword(SessionFacade.CurrentUser.Id, NewPassword, ConfirmPassword);
            userInputViewModel.User.Password = NewPassword;
            userInputViewModel.User.PasswordChangedDate = DateTime.Now;
            userInputViewModel.User.Performer = 1;
            userInputViewModel.User.RegTime = DateTime.Now;
            userInputViewModel.User.ReportPermission = 1;
            userInputViewModel.User.SetPriorityPermission = 1;
            //userInputViewModel.UsersUserRole.User_Id = SessionFacade.CurrentUser.Id;
            userInputViewModel.User.UserGroup_Id = 4;


            this._userService.SaveNewUser(user, AAsSelected, CsSelected, OTsSelected, out errors);

            if (errors.Count == 0)
            {
                return this.RedirectToAction("index", "users");
            }

            var model = this.CreateInputViewModel(userInputViewModel.User);
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
            return this.View(model);
        }

        [CustomAuthorize(Roles = "3,4")]
        public ActionResult Edit(int id, int? copy)
        {

            var user = this._userService.GetUser(id);

            if (user == null)
                return new HttpNotFoundResult("No user found...");

            IDictionary<string, string> errors = new Dictionary<string, string>();

            var model = this.CreateInputViewModel(user);

            if (copy != null)
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
        public ActionResult Edit(int id, int[] AAsSelected, int[] CsSelected, int[] OTsSelected, int[] Departments, List<UserWorkingGroup> UserWorkingGroups, UserSaveViewModel userModel, string NewPassword, string ConfirmPassword, FormCollection coll)
        {
            var userToSave = new User();

            if (id != -1)
            {
                userToSave = this._userService.GetUser(id);
            }

            userToSave.OrderPermission = this.returnOrderPermissionForSave(userModel);
            userToSave.CaseInfoMail = this.returnCaseInfoMailForEditSave(userModel);

            
            var b = this.TryUpdateModel(userToSave, "user");

            if (id == -1)
            {
                userToSave.Id = 0;
                userToSave.Password = NewPassword;
                userToSave.UserRoles = this._userService.GetUserRoles();

                if (userToSave.Language_Id == 0)
                    userToSave.Language_Id = 2;


                foreach (var cu in userToSave.CustomerUsers)
                {
                    cu.User_Id = 0;

                    if (Convert.ToInt32(cu.CaseCaseTypeFilter) > 0)
                    {
                        cu.CaseCaseTypeFilter = "0";
                    }

                    if (Convert.ToInt32(cu.CaseWorkingGroupFilter) > 0)
                    {
                        cu.CaseWorkingGroupFilter = "0";
                    }

                    if (Convert.ToInt32(cu.CaseResponsibleFilter) > 0)
                    {
                        cu.CaseResponsibleFilter = "0";
                    }

                    if (Convert.ToInt32(cu.CaseStateSecondaryFilter) > 0)
                    {
                        cu.CaseStateSecondaryFilter = "0";
                    }

                    if (Convert.ToInt32(cu.CaseProductAreaFilter) > 0 )
                    {
                        cu.CaseProductAreaFilter = "0";
                    }

                    if (Convert.ToInt32(cu.CaseCategoryFilter) > 0)
                    {
                        cu.CaseCategoryFilter = "0";
                    }

                    if (Convert.ToInt32(cu.CaseRegionFilter) > 0)
                    {
                        cu.CaseRegionFilter = "0";
                    }

                    if (Convert.ToInt32(cu.CaseUserFilter) > 0)
                    {
                        cu.CaseUserFilter = "0";
                    }

                    if (Convert.ToInt32(cu.CaseStatusFilter) > 0)
                    {
                        cu.CaseStatusFilter = "0";
                    }

                    if (Convert.ToInt32(cu.CaseResponsibleFilter) > 0 )
                    {
                        cu.CaseResponsibleFilter = "0";
                    }

                    if (Convert.ToInt32(cu.CasePriorityFilter) > 0 )
                    {
                        cu.CasePriorityFilter = "0";
                    }

                    cu.CasePerformerFilter = "0";
                    
                }
               
                
            }

            var vmodel = this.CreateInputViewModel(userToSave);
            vmodel.MenuSetting = userModel.MenuSetting;

            
            if (userToSave.UserRoles != null)
                foreach (var delete in userToSave.UserRoles.ToList())
                    userToSave.UserRoles.Remove(delete);

            if (userModel.UserRights.HasValue)
            {
                var userRight = this._userService.GetUserRoleById(userModel.UserRights.Value);
                userToSave.UserRoles.Add(userRight);
            }


            IDictionary<string, string> errors = new Dictionary<string, string>();

            
            this._userService.SaveEditUser(userToSave, AAsSelected, CsSelected, OTsSelected, Departments, UserWorkingGroups, out errors);

            if (userModel.CopyUserid != 0)
            {
                foreach (var c in userModel.User.CustomerUsers)
                {
                    //Get CaseSettings
                    var caseSettingsToCopy = _caseSettingsService.GetCaseSettingsWithUser(c.Customer_Id, userModel.CopyUserid, userToSave.UserGroup_Id);


                    foreach (var cs in caseSettingsToCopy)
                    {
                        var newUserCaseSetting = new CaseSettings() { };

                        newUserCaseSetting.User_Id = userToSave.Id;
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
                return this.RedirectToAction("index", "users");

            var model = this.CreateInputViewModel(userToSave);


            

           // model.UserColumns = _caseSettingsService.GetCaseSettingsWithUser(userToSave.Customer_Id, userToSave.Id, SessionFacade.CurrentUser.UserGroupId);
            return this.View(model);
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (this._userService.DeleteUser(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "users");
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "users", new { id = id });
            }
        }

        [CustomAuthorize(Roles = "3,4")]
        public ActionResult SignedInUsers()
        {
            return this.PartialView("_SignedInUsers");
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

            var model = new UserIndexViewModel
            {
                User = user,
                StatusUsers = sli,
                ListLoggedInUsers = ApplicationFacade.GetLoggedInUsers(SessionFacade.CurrentCustomer.Id),
                CsSelected = csSelected.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };


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

            var csSelected = user.Cs ?? new List<Customer>();
            var csAvailable = new List<Customer>();

            foreach (var c in this._customerService.GetAllCustomers())
            {
                if (!csSelected.Contains(c))
                    csAvailable.Add(c);
            }

            var otsSelected = user.OTs ?? new List<OrderType>();
            var otsAvailable = new List<OrderType>();

            foreach (var ot in this._orderTypeService.GetOrderTypes(SessionFacade.CurrentCustomer.Id))
            {
                if (!otsSelected.Contains(ot))
                    otsAvailable.Add(ot);
            }

            #endregion

            #region SelectListItems

            List<SelectListItem> lis = new List<SelectListItem>();
            lis.Add(new SelectListItem()
            {
                Text = "Start",
                Value = "1",
                Selected = false
            });
            lis.Add(new SelectListItem()
            {
                Text = "Ärendeöversikt",
                Value = "0",
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

            var model = new UserInputViewModel
            {
                User = user,
                CaseInfoMailList = li,
                RefreshInterval = sli,
                StartPageShowList = lis,
                CustomerUsers = this._userService.GetCustomerUserForUser(user.Id),
                Departments = this._userService.GetDepartmentsForUser(user.Id),
                ListWorkingGroupsForUser = this._userService.GetListToUserWorkingGroup(user.Id),
                //UserColumns = _caseSettingsService.GetCaseSettingsWithUser(user.Customer_Id, user.Id, SessionFacade.CurrentUser.UserGroupId),
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
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                UserGroups = this._userService.GetUserGroups().OrderBy(x => x.Id).Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Name, Enums.TranslationSource.TextTranslation),
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
                CsAvailable = csAvailable.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                CsSelected = csSelected.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
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

            #endregion

            #region SetInts

            if (model.User.Id != 0)
            {
                if (model.User.UserRoles.Any())
                    model.UserRights = model.User.UserRoles.FirstOrDefault().Id;
                else
                    model.UserRights = 0;

                //if (user.MenuSettings != null)
                //{
                //    model.MenuSetting = model.User.MenuSettings.Split(';');
                //}
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

            #endregion

            #region SetStrings




            //if (user.CaseStateSecondaryColor != null)
            //{
            //    if (user.CaseStateSecondaryColor == "#000000")
            //    {
            //        model.StateStatusCase = 1;
            //    }
            //    else if (user.CaseStateSecondaryColor == "#008000")
            //    {
            //        model.StateStatusCase = 2;
            //    }
            //    else
            //    {
            //        model.StateStatusCase = 0;
            //    }
            //}

            #endregion

            return model;
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

        //private UserRole returnUserRoleForNewSave(UserSaveViewModel userModel)
        //{
        //    var userRoles = new UserRole();

        //    if (userModel.UserRights.HasValue)
        //    {
        //         userRoles = this._userService.GetUserRoles();
        //    }

        //    return userRoles;
        //}

        

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

        //private void returnMenuSettingsForSave(UserInputViewModel userInputViewModel, ref User user)
        //{
        //    user.MenuSettings = "";

        //    for (int i = 0; i < userInputViewModel.MenuSetting.Length; i++)
        //    {
        //        user.MenuSettings += i + ":" + userInputViewModel.MenuSetting[i] + ((i == userInputViewModel.MenuSetting.Length - 1) ? "" : ";");
        //    }
        //}

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


        //private string returnCaseStateSecondaryColorForSave(int id, UserSaveViewModel userModel)
        //{
        //    userModel.CaseStateSecondaryColor = "";

        //    if (userModel.StateStatusCase == 1)
        //    {
        //        userModel.CaseStateSecondaryColor = "#000000";
        //    }
        //    else if (userModel.StateStatusCase == 2)
        //    {
        //        userModel.CaseStateSecondaryColor = "#008000";
        //    }
        //    else
        //        userModel.CaseStateSecondaryColor = "";

        //    return userModel.CaseStateSecondaryColor;
        //}

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