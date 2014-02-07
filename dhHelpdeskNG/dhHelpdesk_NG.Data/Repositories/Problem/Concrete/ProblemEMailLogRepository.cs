namespace DH.Helpdesk.Dal.Repositories.Problem.Concrete
{
    using System.Linq;

    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ProblemEMailLogRepository : Repository, IProblemEMailLogRepository
    {
        public ProblemEMailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void DeleteByLogId(int logId)
        {
            var emailLogs = this.DbContext.ProblemEMailLogs.Where(x => x.ProblemLog_Id == logId).ToList();
            emailLogs.ForEach(x => this.DbContext.ProblemEMailLogs.Remove(x));
        }

        public void DeleteByProblemId(int problemId)
        {
            var emailLogs = this.DbContext.ProblemEMailLogs.Where(x => x.ProblemLog.Problem_Id == problemId).ToList();
            emailLogs.ForEach(x => this.DbContext.ProblemEMailLogs.Remove(x));
        }
    }
}