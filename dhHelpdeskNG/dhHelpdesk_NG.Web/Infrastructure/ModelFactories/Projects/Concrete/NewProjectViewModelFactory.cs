namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Projects.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Web.Models.Projects;

    public class NewProjectViewModelFactory : INewProjectViewModelFactory
    {
        public NewProjectViewModel Create(List<User> users)
        {
            return new NewProjectViewModel
                       {
                           Project = new ProjectEditModel { ProjectCollaborators = new List<SelectListItem>() },
                           Users = users.Select(x => new SelectListItem { Text = string.Format("{0} {1}", x.FirstName, x.SurName), Value = x.Id.ToString(CultureInfo.InvariantCulture) }).ToList()
                       };
        }
    }
}