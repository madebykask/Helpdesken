﻿namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Projects.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Web.Models.Projects;

    public class NewProjectScheduleFactory : INewProjectScheduleFactory
    {
        public NewProjectSchedule Create(ProjectScheduleEditModel editModel, DateTime createTime)
        {
            return new NewProjectSchedule(
                editModel.ProjectId,
                editModel.UserId,
                editModel.Name,
                editModel.Position,
                editModel.Time,
                editModel.Description,
                editModel.StartDate,
                editModel.FinishDate,
                editModel.CaseNumber,
                createTime);
        }
    }
}