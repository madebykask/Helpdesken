namespace DH.Helpdesk.Web.Models.Profile
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Users.Output;

    public sealed class UserModulesViewModel
    {
        private IEnumerable<UserModuleOverview> modules = new UserModuleOverview[] { };

        public IEnumerable<UserModuleOverview> Modules
        {
            get { return this.modules; }
            set { this.modules = value; }
        }
    }
}