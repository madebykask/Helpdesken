namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Projects
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Mobile.Models.Projects;

    public interface IUpdatedProjectScheduleFactory
    {
        UpdatedProjectSchedule Create(ProjectScheduleEditModel editModel, DateTime changeTime);
    }
}