namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Projects
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Models.Projects;

    public interface INewProjectViewModelFactory
    {
        NewProjectViewModel Create(List<User> users, string guid);
    }
}