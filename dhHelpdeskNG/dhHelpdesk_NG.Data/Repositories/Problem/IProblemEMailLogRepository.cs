namespace dhHelpdesk_NG.Data.Repositories.Problem
{
    using dhHelpdesk_NG.Data.Dal;

    public interface IProblemEMailLogRepository : INewRepository
    {
        void DeleteByLogId(int logId);

        void DeleteByProblemId(int problemId);
    }
}