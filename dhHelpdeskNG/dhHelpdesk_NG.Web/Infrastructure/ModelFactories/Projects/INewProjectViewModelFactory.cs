namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Projects
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Models.Projects;
    using System.Web.Mvc;

    public interface INewProjectViewModelFactory
    {
        NewProjectViewModel Create(SelectList users, string guid);
    }
}