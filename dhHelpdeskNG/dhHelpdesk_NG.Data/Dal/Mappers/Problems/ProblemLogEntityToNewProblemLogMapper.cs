namespace dhHelpdesk_NG.Data.Dal.Mappers.Problems
{
    using dhHelpdesk_NG.Domain.Problems;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;

    public class ProblemLogEntityToNewProblemLogMapper : IEntityToBusinessModelMapper<ProblemLog, NewProblemLogDto>
    {
        public NewProblemLogDto Map(ProblemLog entity)
        {
            return new NewProblemLogDto(
                entity.ChangedByUser_Id,
                entity.LogText,
                entity.ShowOnCase,
                entity.FinishingCause_Id,
                entity.FinishingDate,
                entity.FinishConnectedCases)
                       {
                           Id = entity.Id,
                           ProblemId = entity.Problem_Id
                       };
        }
    }
}