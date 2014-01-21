using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Areas.Admin.Models;   

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
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
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _accountActivityService = accountActivityService;
            _customerService = customerService;
            _customerUserService = customerUserService;
            _departmentService = departmentService;
            _domainService = domainService;
            _languageService = languageService;
            _orderTypeService = orderTypeService;
            _userService = userService;
            _workingGroupService = workingGroupService;
        }

        [CustomAuthorize(Roles = "3,4")]
        public ActionResult Index()
        {
            var model = IndexInputViewModel();
            model.Users = _userService.GetUsers(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.UserID).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(int? StatusId, UserSearch SearchUsers)
        {
            var model = IndexInputViewModel();
            model.Users =_userService.SearchSortAndGenerateUsers(SessionFacade.CurrentUser.Customer_Id, StatusId, SearchUsers);
            return View(model);
        }

        public ActionResult New()
        {
            var model = CreateInputViewModel(new User
            {
                ActivateCasePermission = 1,
                BulletinBoardDate = DateTime.Now,
                ChangeTime = DateTime.Now,
                CloseCasePermission = 1,
                CopyCasePermission = 1,
                DeleteCasePermission = 1,
                FAQPermission = 1,
                IsActive = 1,
                Language_Id = SessionFacade.CurrentLanguage,
                MenuSettings = string.Empty,
                MoveCasePermission = 1,
                PasswordChangedDate = DateTime.Now,
                Performer = 1,
                RefreshContent = 300,
                RegTime = DateTime.Now,
                ReportPermission = 1,
                SessionTimeout = 120,
                SetPriorityPermission = 1,
                ShowNotAssignedWorkingGroups = 1,
                StartPage = 1
            });

            return View(model);
        }

        [HttpPost]
        public ActionResult New(UserInputViewModel userInputViewModel, int[] AAsSelected, int[] CsSelected, int[] OTsSelected, string NewPassword, string ConfirmPassword, FormCollection coll)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            var user = returnCaseInfoMailForNewSave(userInputViewModel);

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
            userInputViewModel.User.Password = NewUserPassword(SessionFacade.CurrentUser.Id, NewPassword, ConfirmPassword);
            userInputViewModel.User.PasswordChangedDate = DateTime.Now;
            userInputViewModel.User.Performer = 1;
            userInputViewModel.User.RegTime = DateTime.Now;
            userInputViewModel.User.ReportPermission = 1;
            userInputViewModel.User.SetPriorityPermission = 1;
            userInputViewModel.UsersUserRole.User_Id = SessionFacade.CurrentUser.Id;

            _userService.SaveNewUser(user, AAsSelected, CsSelected, OTsSelected, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "users");

            return View(user);
        }

        public ActionResult Edit(int id)
        {
            var user = _userService.GetUser(id);

            if (user == null)
                return new HttpNotFoundResult("No user found...");

            var model = CreateInputViewModel(user);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, int[] AAsSelected, int[] CsSelected, int[] OTsSelected, int[] Departments, List<UserWorkingGroup> UserWorkingGroups, UserSaveViewModel userModel, FormCollection coll)
        {
            var userToSave = _userService.GetUser(id);
            //userToSave.CaseStateSecondaryColor = returnCaseStateSecondaryColorForSave(id, userModel);
            userToSave.OrderPermission = returnOrderPermissionForSave(id, userModel);
            userToSave.CaseInfoMail = returnCaseInfoMailForEditSave(id, userModel);

            var b = TryUpdateModel(userToSave, "user");
            var vmodel = CreateInputViewModel(userToSave);
            vmodel.MenuSetting = userModel.MenuSetting;
            

            if (userToSave.UserRoles != null)
                foreach (var delete in userToSave.UserRoles.ToList())
                    userToSave.UserRoles.Remove(delete);

            if (userModel.UserRights.HasValue) //TODO: Save userrole correct! geht nichts momental
            {
                var userRight = _userService.GetUserRoleById(userModel.UserRights.Value);
                userToSave.UserRoles.Add(userRight);
            }

            returnMenuSettingsForSave(vmodel, ref userToSave);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            _userService.SaveEditUser(userToSave, AAsSelected, CsSelected, OTsSelected, Departments, UserWorkingGroups, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "users");

            var model = CreateInputViewModel(userToSave);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (_userService.DeleteUser(id) == DeleteMessage.Success)
                return RedirectToAction("index", "users");
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "users", new { id = id });
            }
        }

        public ActionResult SignedInUsers()
        {
            return PartialView("_SignedInUsers");
        }

        private UserIndexViewModel IndexInputViewModel()
        {
            var user = _userService.GetUser(SessionFacade.CurrentUser.Id);

            var csSelected = user.Cs ?? new List<Customer>();
            var csAvailable = new List<Customer>();
            

            foreach (var c in _customerService.GetAllCustomers())
            {
                if (!csSelected.Contains(c))
                    csAvailable.Add(c);
            }

            List<SelectListItem> sli = new List<SelectListItem>();
            sli.Add(new SelectListItem()
            {
                Text = Translation.Get("Aktiva", Enums.TranslationSource.TextTranslation),
                Value = "1",
                Selected = true
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
                StatusUsers = sli,
                ListLoggedOnUsers = _userService.GetListToUserLoggedOn(), // när ska man använda SessionFacade.SignedInUser???
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

            foreach (var aa in _accountActivityService.GetAccountActivities(SessionFacade.CurrentCustomer.Id))
            {
                if (!aasSelected.Contains(aa))
                    aasAvailable.Add(aa);
            }

            var csSelected = user.Cs ?? new List<Customer>();
            var csAvailable = new List<Customer>();

            foreach (var c in _customerService.GetAllCustomers())
            {
                if (!csSelected.Contains(c))
                    csAvailable.Add(c);
            }

            var otsSelected = user.OTs ?? new List<OrderType>();
            var otsAvailable = new List<OrderType>();

            foreach (var ot in _orderTypeService.GetOrderTypes(SessionFacade.CurrentCustomer.Id))
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
                Text = "Varje vecka",
                Value = "1",
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = "Varje dag",
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
                CustomerUsers = _userService.GetCustomerUserForUser(SessionFacade.CurrentUser.Id),
                Departments = _userService.GetDepartmentsForUser(SessionFacade.CurrentUser.Id),
                ListWorkingGroupsForUser = _userService.GetListToUserWorkingGroup(SessionFacade.CurrentUser.Id),
                Customers = _customerService.GetAllCustomers().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Domains = _domainService.GetDomains(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Languages = _languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                UserGroups = _userService.GetUserGroups().OrderBy(x => x.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                UserRoles = _userService.GetUserRoles().Select(x => new SelectListItem
                {
                    Text = x.Description,
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
                WorkingGroups = _workingGroupService.GetAllWorkingGroups().Select(x => new SelectListItem
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

                if (user.MenuSettings != null)
                {
                    model.MenuSetting = model.User.MenuSettings.Split(';');
                }
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

        //private UsersUserRole returnUserRoleForNewSave(UserInputViewModel userInputViewModel)
        //{
        //    var usersUserRole = userInputViewModel.UsersUserRole;

        //    if (userInputViewModel.UserRights.HasValue)
        //    {

        //    }

        //    return usersUserRole;
        //}

        private int returnCaseInfoMailForEditSave(int id, UserSaveViewModel userModel)
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

        private int returnOrderPermissionForSave(int id, UserSaveViewModel userModel)
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

        private void returnMenuSettingsForSave(UserInputViewModel userInputViewModel, ref User user)
        {
            user.MenuSettings = "";

            for (int i = 0; i < userInputViewModel.MenuSetting.Length; i++)
            {
                user.MenuSettings += i + ":" + userInputViewModel.MenuSetting[i] + ((i == userInputViewModel.MenuSetting.Length - 1) ? "" : ";");
            }
        }

        private string NewUserPassword(int id, string newPassword, string confirmPassword)
        {
            var user = _userService.GetUser(id);

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
                _userService.SavePassword(id, newPassword);
            }
            else
                throw new ArgumentNullException("The password fields do not  match, please re-type them...");
        }
    }
}