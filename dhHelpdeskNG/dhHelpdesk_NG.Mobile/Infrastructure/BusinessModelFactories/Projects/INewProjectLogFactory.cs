namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Projects
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Web.Models.Projects;

    public interface INewProjectLogFactory
    {
        NewProjectLog Create(ProjectLogEditModel editModel, DateTime createdTime);
    }
}