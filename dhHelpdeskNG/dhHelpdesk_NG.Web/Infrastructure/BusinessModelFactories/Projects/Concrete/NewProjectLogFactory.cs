namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Projects.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Web.Models.Projects;

    public class NewProjectLogFactory : INewProjectLogFactory
    {
        public NewProjectLog Create(ProjectLogEditModel editModel, DateTime createdTime)
        {
            return new NewProjectLog(editModel.ProjectId, editModel.LogText, editModel.ResponsibleUserId, createdTime);
        }
    }
}