namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Projects.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Web.Models.Projects;

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
                editModel.StartDate == null ? null : (DateTime?)DateTime.Parse(editModel.StartDate),
                editModel.EndDate == null ? null : (DateTime?)DateTime.Parse(editModel.EndDate),
                createdTime);
        }
    }
}