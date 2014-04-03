using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Inventory.Input;
using DH.Helpdesk.Dal.EntityConfigurations.Questionnaire;
using DH.Helpdesk.Services.utils;
using DH.Helpdesk.Web.Infrastructure.WorkContext;
using Ninject;

namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Models.Profile;
    using DH.Helpdesk.Web.Infrastructure;
   

    public class ProfileController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;

        public ProfileController(
            
            IUserService userService,
            IMasterDataService masterDataService,
            IWorkContext workContext)
            : base(masterDataService)
        {
            this._userService = userService;
            _workContext = workContext;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {

            var user = this._userService.GetUser(id);

            if (user == null)
                return new HttpNotFoundResult("No user found...");


            var modules = new UserModulesViewModel();
            modules.Modules = _workContext.User.Modules;            
            var model = this.CreateInputViewModel(user, modules);

            return this.View(model);


        }

        [HttpPost]
        public ActionResult Edit(int id, 
                        string NewPassword, 
                        string ConfirmPassword, 
                        ProfileSaveViewModel profileUserModel, 
                        FormCollection coll)
        {
            var userToSave = new User();

            if (id != -1)
            {
                userToSave = this._userService.GetUser(id);
            }

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
                }

            }

            var vmodel = this.CreateInputViewModel(userToSave);
           
            if (userToSave.UserRoles != null)
                foreach (var delete in userToSave.UserRoles.ToList())
                    userToSave.UserRoles.Remove(delete);

            if (profileUserModel.UserRights.HasValue)
            {
                var userRight = this._userService.GetUserRoleById(profileUserModel.UserRights.Value);
                userToSave.UserRoles.Add(userRight);
            }


            IDictionary<string, string> errors = new Dictionary<string, string>();


            this._userService.SaveProfileUser(userToSave, out errors);
            _userService.UpdateUserModules(profileUserModel.Modules);
            _workContext.User.Modules = null;

            if (errors.Count == 0)
                return this.RedirectToAction("edit", "profile", new { id = userToSave.Id});

            var model = this.CreateInputViewModel(userToSave);

            return this.View(model);
        }

        private ProfileInputViewModel CreateInputViewModel(User user, UserModulesViewModel modules = null)
        {
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

            var model = new ProfileInputViewModel
            {
                User = user,
                RefreshInterval = sli,
                Modules = modules
            };

            return model;
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
