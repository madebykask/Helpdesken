namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Projects
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Mobile.Models.Projects;

    public interface INewProjectScheduleFactory
    {
        NewProjectSchedule Create(ProjectScheduleEditModel editModel, DateTime createTime);
    }
}