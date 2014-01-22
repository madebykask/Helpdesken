namespace dhHelpdesk_NG.Data.Dal.Mappers.Problems
{
    using System;

    using dhHelpdesk_NG.Domain.Problems;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;

    public class ProblemLogEntityFromBusinessModelChanger : IEntityChangerFromBusinessModel<NewProblemLogDto, ProblemLog>
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