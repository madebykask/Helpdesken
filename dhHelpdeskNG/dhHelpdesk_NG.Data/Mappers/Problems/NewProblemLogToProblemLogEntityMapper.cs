namespace DH.Helpdesk.Dal.Mappers.Problems
{
    using DH.Helpdesk.BusinessData.Models.Problem.Input;
    using DH.Helpdesk.Domain.Problems;

    public class NewProblemLogToProblemLogEntityMapper : INewBusinessModelToEntityMapper<NewProblemLogDto, ProblemLog>
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