namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Projects
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Mobile.Models.Projects;

    public interface INewProjectViewModelFactory
    {
        NewProjectViewModel Create(List<User> users, string guid);
    }
}