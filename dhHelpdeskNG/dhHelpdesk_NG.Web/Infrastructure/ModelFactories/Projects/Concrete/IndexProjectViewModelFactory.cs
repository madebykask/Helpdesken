namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Projects.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;
    using dhHelpdesk_NG.Web.Infrastructure.Filters.Projects;
    using dhHelpdesk_NG.Web.Models.Projects;

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