namespace dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Projects.Concrete
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.Web.Models.Projects;

    public class NewProjectLogFactory : INewProjectLogFactory
    {
        public NewProjectLog Create(ProjectLogEditModel editModel, DateTime createdTime)
        {
            return new NewProjectLog(editModel.ProjectId, editModel.LogText, editModel.ResponsibleUserId, createdTime);
        }
    }
}