namespace dhHelpdesk_NG.Data.Dal.Mappers.Problems
{
    using dhHelpdesk_NG.Domain.Problems;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;

    public class ProblemLogBusinessModelToEntityMapper : IBusinessModelToEntityMapper<NewProblemLogDto, ProblemLog>
    {
        public ProblemLog Map(NewProblemLogDto businessModel)
        {
            return new ProblemLog
                       {
                           Id = businessModel.Id,
                           Problem_Id = businessModel.ProblemId,
                           ChangedByUser_Id = businessModel.ChangedByUserId,
                           LogText = businessModel.LogText,
                           ShowOnCase = businessModel.ShowOnCase,
                           FinishingCause_Id = businessModel.FinishingCauseId,
                           FinishingDate = businessModel.FinishingDate,
                           FinishConnectedCases = businessModel.FinishConnectedCases
                       };
        }
    }
}