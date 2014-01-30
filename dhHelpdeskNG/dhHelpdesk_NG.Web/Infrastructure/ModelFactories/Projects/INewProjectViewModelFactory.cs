namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Projects
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Web.Models.Projects;

    public interface INewProjectViewModelFactory
    {
        NewProjectViewModel Create(List<User> users, string guid);
    }
}