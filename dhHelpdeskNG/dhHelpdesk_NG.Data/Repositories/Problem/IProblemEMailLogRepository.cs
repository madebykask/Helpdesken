namespace dhHelpdesk_NG.Data.Repositories.Problem
{
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Problems;

    public interface IProblemEMailLogRepository : IRepository<ProblemEMailLog>
    {
        void DeleteByLogId(int logId);

        void DeleteByProblemId(int problemId);
    }
}