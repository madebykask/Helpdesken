using DH.Helpdesk.BusinessData.Models.Users.Input;
using DH.Helpdesk.BusinessData.Models.Users.Output;

namespace DH.Helpdesk.Mobile.Models.Profile
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Domain;

    public class ChangeUserPasswordModel
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public User User { get; set; }
    }

    
    public class ProfileInputViewModel
    {
        public User User { get; set; }
        public string ConfirmPassword { get; set; }
        public string NewPassword { get; set; }
        public IList<SelectListItem> RefreshInterval { get; set; }

        public ProfileInputViewModel() { }

        private UserModulesViewModel _modules = new UserModulesViewModel();
        public UserModulesViewModel Modules
        {
            get { return _modules; }
            set { _modules = value; }
        }
    }

    public class ProfileSaveViewModel
    {
        public User User { get; set; }

        public int OrderP1 { get; set; }
        public int OrderP2 { get; set; }
        public int UserOrderPermission { get; set; }
        public int SendMailYesNo { get; set; }
        public int? SendMailYesNoWhen { get; set; }
        public int StateStatusCase { get; set; }
        public int? UserRights { get; set; }
        //public string CaseStateSecondaryColor { get; set; }
        public string[] MenuSetting { get; set; }

        private IList<UserModule> _modules = new List<UserModule>();
        public IList<UserModule> Modules
        {
            get { return _modules; }
            set { _modules = value; }
        }
    }
}