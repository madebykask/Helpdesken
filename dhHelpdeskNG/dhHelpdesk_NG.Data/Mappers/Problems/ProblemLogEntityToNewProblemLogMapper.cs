namespace DH.Helpdesk.Dal.Mappers.Problems
{
    using DH.Helpdesk.BusinessData.Models.Problem.Input;
    using DH.Helpdesk.Domain.Problems;

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