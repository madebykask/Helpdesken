namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Projects.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Infrastructure.Filters.Projects;
    using DH.Helpdesk.Web.Models.Projects;

    public class IndexProjectViewModelFactory : IIndexProjectViewModelFactory
    {
        public IndexProjectViewModel Create(List<ProjectOverview> overviews, SelectList users, ProjectFilter filter)
        {
            return new IndexProjectViewModel
                       {
                           Projects = overviews ?? new List<ProjectOverview>(),
                           Filter = filter ?? new ProjectFilter(),
                           Users = users.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList()
                       };
        }
    }
}