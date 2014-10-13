namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Projects.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Mobile.Infrastructure.Filters.Projects;
    using DH.Helpdesk.Mobile.Models.Projects;

    public class IndexProjectViewModelFactory : IIndexProjectViewModelFactory
    {
        public IndexProjectViewModel Create(List<ProjectOverview> overviews, List<User> users, ProjectFilter filter)
        {
            return new IndexProjectViewModel
                       {
                           Projects = overviews ?? new List<ProjectOverview>(),
                           Filter = filter ?? new ProjectFilter(),
                           Users = users.Select(x => new SelectListItem { Text = string.Format("{0} {1}", x.FirstName, x.SurName), Value = x.Id.ToString(CultureInfo.InvariantCulture) }).ToList()
                       };
        }
    }
}