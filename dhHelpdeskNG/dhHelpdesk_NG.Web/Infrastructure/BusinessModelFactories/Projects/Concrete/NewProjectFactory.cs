namespace dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Projects.Concrete
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.Web.Models.Projects;

    public class NewProjectFactory : INewProjectFactory
    {
        public NewProject Create(ProjectEditModel editModel, int customerId, DateTime createdTime)
        {
            return new NewProject(
                editModel.Name,
                customerId,
                editModel.ProjectManagerId,
                editModel.IsActive,
                editModel.Description,
                editModel.EndDate == null ? null : (DateTime?)DateTime.Parse(editModel.EndDate),
                createdTime);
        }
    }
}