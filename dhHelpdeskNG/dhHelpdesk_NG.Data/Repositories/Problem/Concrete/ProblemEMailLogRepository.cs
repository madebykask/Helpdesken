namespace dhHelpdesk_NG.Data.Repositories.Problem.Concrete
{
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Problems;

    public class ProblemEMailLogRepository : RepositoryBase<ProblemEMailLog>, IProblemEMailLogRepository
    {
        public ProblemEMailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void DeleteByLogId(int logId)
        {
            var emailLogs =
                this.DataContext.ProblemEMailLogs.Where(x => x.ProblemLog_Id == logId).ToList();

            emailLogs.ForEach(x => this.DataContext.ProblemEMailLogs.Remove(x));
        }

        public void DeleteByProblemId(int problemId)
        {
            var emailLogs =
                this.DataContext.ProblemEMailLogs.Where(x => x.ProblemLog.Problem_Id == problemId).ToList();

            emailLogs.ForEach(x => this.DataContext.ProblemEMailLogs.Remove(x));
        }
    }
}