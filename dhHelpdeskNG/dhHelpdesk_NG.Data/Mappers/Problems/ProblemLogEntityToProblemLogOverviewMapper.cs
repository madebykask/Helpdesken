namespace DH.Helpdesk.Dal.Mappers.Problems
{
    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.Domain.Problems;

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