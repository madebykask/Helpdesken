namespace dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Projects
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.Web.Models.Projects;

    public interface INewProjectScheduleFactory
    {
        NewProjectSchedule Create(ProjectScheduleEditModel editModel, DateTime createTime);
    }
}