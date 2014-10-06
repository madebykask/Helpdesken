namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Projects
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Mobile.Models.Projects;

    public interface INewProjectLogFactory
    {
        NewProjectLog Create(ProjectLogEditModel editModel, DateTime createdTime);
    }
}