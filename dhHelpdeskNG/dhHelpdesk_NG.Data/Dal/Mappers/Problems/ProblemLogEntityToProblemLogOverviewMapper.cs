namespace dhHelpdesk_NG.Data.Dal.Mappers.Problems
{
    using dhHelpdesk_NG.Domain.Problems;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public class ProblemLogEntityToProblemLogOverviewMapper : IEntityToBusinessModelMapper<ProblemLog, ProblemLogOverview>
    {
        public ProblemLogOverview Map(ProblemLog entity)
        {
            return new ProblemLogOverview
                       {
                           Id = entity.Id,
                           ChangedByUserName = string.Format("{0} {1}", entity.ChangedByUser.FirstName, entity.ChangedByUser.SurName),
                           ChangedDate = entity.ChangedDate,
                           LogText = entity.LogText,
                       };
        }
    }
}