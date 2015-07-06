namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Users.Output;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models.Profile;

    public class ProfileController : BaseController
    {
        private readonly IUserService userService;

        private readonly IWorkContext workContext;

        private readonly IModulesService moduleService;

        public ProfileController(            
            IUserService userService,
            IMasterDataService masterDataService,
            IModulesService moduleService,
            IWorkContext workContext)
            : base(masterDataService)
        {
            this.userService = userService;
            this.workContext = workContext;
            this.moduleService = moduleService;
        }

        public ActionResult Edit()
        {
            var user = this.userService.GetUser(SessionFacade.CurrentUser.Id);

            if (user == null)
            {
                return new HttpNotFoundResult("No user found...");
            }

            user.TimeZoneId = SessionFacade.CurrentUser.TimeZoneId ?? TimeZoneInfo.Local.Id;

            var allModules = this.moduleService.GetAllModules();                                                
                                          
            var modules = new UserModulesViewModel();
            var userModules = this.workContext.User.Modules;
                   
            var primaryModules = new List<UserModuleOverview>();

            foreach (var mod in allModules)
            {                
                var curUserModule = userModules.Where(u=> u.Module_Id == mod.Id).SingleOrDefault();

                var newMod = new UserModuleOverview();

                var subMod = new ModuleOverview
                                {  
                                   Id = mod.Id,
                                   Name = Translation.Get(mod.Name),
                                   Description = mod.Description
                                };

                newMod.Module = subMod;
                
                newMod.Id = mod.Id;
                if (curUserModule == null)
                {
                    newMod.isVisible = false;
                    newMod.NumberOfRows = 3;
                    newMod.Position = 303;
                    newMod.User_Id = SessionFacade.CurrentUser.Id;
                    newMod.Module_Id = mod.Id;                    
                }
                else
                {
                    newMod.isVisible = curUserModule.isVisible;
                    newMod.NumberOfRows = curUserModule.NumberOfRows;
                    newMod.Position = curUserModule.Position;
                    newMod.User_Id = curUserModule.User_Id;
                    newMod.Module_Id = curUserModule.Module_Id;                                        
                }

                primaryModules.Add(newMod);
            }

            modules.Modules = primaryModules;
            var customerSettings = this.userService.GetUserProfileCustomersSettings(user.Id);
            var customerSettingsModel = new UserCustomersSettingsViewModel(customerSettings);
            var model = this.CreateInputViewModel(user, modules, customerSettingsModel);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(
                        int id, 
                        string NewPassword, 
                        string ConfirmPassword, 
                        ProfileSaveViewModel profileUserModel, 
                        FormCollection coll)
        {
            var userToSave = new User();

            if (id != -1)
            {
                userToSave = this.userService.GetUser(id);
            }

            var b = this.TryUpdateModel(userToSave, "user");

            if (id == -1)
            {
                userToSave.Id = 0;
                userToSave.Password = NewPassword;
                userToSave.UserRoles = this.userService.GetUserRoles();

                if (userToSave.Language_Id == 0)
                {
                    userToSave.Language_Id = 2;
                }

                foreach (var cu in userToSave.CustomerUsers)
                {
                    cu.User_Id = 0;
                }
            }
           
            if (userToSave.UserRoles != null)
            {
                foreach (var delete in userToSave.UserRoles.ToList())
                {
                    userToSave.UserRoles.Remove(delete);
                }
            }

            if (profileUserModel.UserRights.HasValue)
            {
                var userRight = this.userService.GetUserRoleById(profileUserModel.UserRights.Value);
                userToSave.UserRoles.Add(userRight);
            }

            IDictionary<string, string> errors = new Dictionary<string, string>();

            userToSave.TimeZoneId = profileUserModel.SelectedTimeZone;

            this.userService.SaveProfileUser(userToSave, out errors);

            if (errors.Count == 0)
            {
                this.userService.UpdateUserModules(profileUserModel.Modules);
                this.userService.UpdateUserProfileCustomerSettings(id, profileUserModel.CustomersSettings);
                this.workContext.Refresh();

                return this.RedirectToAction("edit", "profile", new { id = userToSave.Id});
            }

            var model = this.CreateInputViewModel(userToSave);

            return this.View(model);
        }

        [HttpPost]
        public void EditUserPassword(int id, string newPassword, string confirmPassword)
        {
            if (newPassword == confirmPassword)
            {
                this.userService.SavePassword(id, newPassword);
            }
            else
            {
                throw new ArgumentNullException("The password fields do not  match, please re-type them...");
            }
        }

        private ProfileInputViewModel CreateInputViewModel(
                                    User user, 
                                    UserModulesViewModel modules = null, 
                                    UserCustomersSettingsViewModel customersSettings = null)
        {
            var sli = new List<SelectListItem>();
            sli.Add(new SelectListItem
            {
                Text = string.Empty,
                Value = "0",
                Selected = false
            });
            sli.Add(new SelectListItem
            {
                Text = "1",
                Value = "60",
                Selected = false
            });
            sli.Add(new SelectListItem
            {
                Text = "2",
                Value = "120",
                Selected = false
            });
            sli.Add(new SelectListItem
            {
                Text = "3",
                Value = "180",
                Selected = false
            });
            sli.Add(new SelectListItem
            {
                Text = "4",
                Value = "240",
                Selected = false
            });
            sli.Add(new SelectListItem
            {
                Text = "5",
                Value = "300",
                Selected = false
            });
            sli.Add(new SelectListItem
            {
                Text = "10",
                Value = "600",
                Selected = false
            });
            sli.Add(new SelectListItem
            {
                Text = "15",
                Value = "900",
                Selected = false
            });

            var model = new ProfileInputViewModel
            {
                User = user,
                RefreshInterval = sli,
                Modules = modules,
                CustomersSettings = customersSettings,
                AvailvableTimeZones = TimeZoneInfo.GetSystemTimeZones().Select(it => new SelectListItem() { Value = it.Id, Text = it.DisplayName, Selected = user.TimeZoneId == it.Id }),
                SelectedTimeZone = user.TimeZoneId
            };

            return model;
        }
    }
}
