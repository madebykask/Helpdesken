namespace dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Projects.Concrete
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.Web.Models.Projects;

    public class UpdatedProjectFactory : IUpdatedProjectFactory
    {
        public UpdatedProject Create(ProjectEditModel editModel, DateTime changeTime)
        {
            return new UpdatedProject(
                editModel.Id,
                editModel.Name,
                editModel.ProjectManagerId,
                editModel.IsActive,
                editModel.Description,
                editModel.EndDate == null ? null : (DateTime?)DateTime.Parse(editModel.EndDate),
                changeTime);
        }
    }
}