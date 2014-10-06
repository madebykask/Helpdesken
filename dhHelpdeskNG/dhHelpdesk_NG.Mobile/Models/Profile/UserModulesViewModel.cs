using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Users.Output;

namespace DH.Helpdesk.Mobile.Models.Profile
{
    public sealed class UserModulesViewModel
    {
        private IEnumerable<UserModuleOverview> _modules = new UserModuleOverview[] {};
        public IEnumerable<UserModuleOverview> Modules
        {
            get { return _modules; }
            set { _modules = value; }
        }
    }
}