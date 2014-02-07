namespace DH.Helpdesk.Dal.Dal.Mappers.Problems
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Problem.Input;
    using DH.Helpdesk.Domain.Problems;

    public class ProblemLogEntityFromBusinessModelChanger : IBusinessModelToEntityMapper<NewProblemLogDto, ProblemLog>
    {
        public void Map(NewProblemLogDto businessModel, ProblemLog entity)
        {
            entity.LogText = businessModel.LogText;
            entity.ShowOnCase = businessModel.ShowOnCase;
            entity.FinishingCause_Id = businessModel.FinishingCauseId;
            entity.FinishingDate = businessModel.FinishingDate;
            entity.FinishConnectedCases = businessModel.FinishConnectedCases;
            entity.ChangedDate = DateTime.Now;
        }
    }
}