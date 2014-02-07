namespace DH.Helpdesk.Dal.Repositories.Problem
{
    using DH.Helpdesk.Dal.Dal;

    public interface IProblemEMailLogRepository : INewRepository
    {
        void DeleteByLogId(int logId);

        void DeleteByProblemId(int problemId);
    }
}