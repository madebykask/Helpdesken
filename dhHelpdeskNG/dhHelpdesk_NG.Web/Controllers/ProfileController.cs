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
            if (SessionFacade.CurrentUser == null)
            {
                return this.RedirectToAction("Unathorized", "Error");
            }

            var user = this.userService.GetUser(SessionFacade.CurrentUser.Id);
            if (user == null)
            {
                return this.RedirectToAction("Unathorized", "Error");
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

            modules.Modules = primaryModules.OrderBy(x => x.Module.Name);
            var customerSettings = this.userService.GetUserProfileCustomersSettings(user.Id);
            var customerSettingsModel = new UserCustomersSettingsViewModel(customerSettings);
            var model = this.CreateInputViewModel(user, modules, customerSettingsModel);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(
                        string NewPassword, 
                        string ConfirmPassword, 
                        ProfileSaveViewModel profileUserModel, 
                        FormCollection coll)
        {
            if (SessionFacade.CurrentUser == null)
            {
                return this.RedirectToAction("Unathorized", "Error");
            }

            var userToSave = this.userService.GetUser(SessionFacade.CurrentUser.Id);
            if (userToSave == null)
            {
                return this.RedirectToAction("Unathorized", "Error");
            }

            var b = this.TryUpdateModel(userToSave, "user");
            
            if (userToSave.UserRoles != null)
            {
                foreach (var delete in userToSave.UserRoles.ToList())
                {
                    userToSave.UserRoles.Remove(delete);
                }
            }

            if (profileUserModel.UserRights.HasValue && userToSave.UserRoles != null)
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
                this.userService.UpdateUserProfileCustomerSettings(SessionFacade.CurrentUser.Id, profileUserModel.CustomersSettings);
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
            var intervals = new List<int> { 0, 1, 2, 3, 4, 5, 10, 15 };

            var model = new ProfileInputViewModel
            {
                User = user,

                RefreshInterval = intervals.Select(x => new SelectListItem()
                {
                    Text = x.ToString(),
                    Value =  (x * 60).ToString()
                }).ToList(),

                Modules = modules,
                CustomersSettings = customersSettings,

                AvailvableTimeZones = TimeZoneInfo.GetSystemTimeZones().Select(it => new SelectListItem()
                {
                    Value = it.Id,
                    Text = it.DisplayName,
                    Selected = user.TimeZoneId == it.Id
                }),

                SelectedTimeZone = user.TimeZoneId
            };

            return model;
        }
    }
}
