namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Projects.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Mobile.Models.Projects;

    public class UpdatedProjectFactory : IUpdatedProjectFactory
    {
        public UpdatedProject Create(ProjectEditModel editModel, DateTime changeTime)
        {
            return new UpdatedProject(
                editModel.Id,
                editModel.Name,
                editModel.ProjectManagerId,
                editModel.IsActive ? 1 : 0,
                editModel.Description,
                editModel.StartDate = editModel.StartDate,
                editModel.EndDate = editModel.EndDate,
                changeTime);
        }
    }
}