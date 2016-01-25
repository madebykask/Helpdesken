namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Projects.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Web.Models.Projects;

    public class UpdatedProjectScheduleFactory : IUpdatedProjectScheduleFactory
    {
        public UpdatedProjectSchedule Create(ProjectScheduleEditModel editModel, DateTime changeTime)
        {
            return new UpdatedProjectSchedule(
                editModel.Id,
                editModel.UserId,
                editModel.Name,
                editModel.Position,
                editModel.State == null ? 0 : (int)editModel.State.Value,
                editModel.Time,
                editModel.Description,
                editModel.StartDate,
                editModel.FinishDate,
                editModel.CaseNumber,
                changeTime);
        }
    }
}