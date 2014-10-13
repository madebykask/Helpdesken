namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Projects.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Mobile.Models.Projects;

    public class NewProjectFactory : INewProjectFactory
    {
        public NewProject Create(ProjectEditModel editModel, int customerId, DateTime createdTime)
        {
            return new NewProject(
                editModel.Name,
                customerId,
                editModel.ProjectManagerId,
                editModel.IsActive ? 1 : 0,
                editModel.Description,
                editModel.StartDate = editModel.StartDate,
                editModel.EndDate = editModel.EndDate,
                createdTime);
        }
    }
}