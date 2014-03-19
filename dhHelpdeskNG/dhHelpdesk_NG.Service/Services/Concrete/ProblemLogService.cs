namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Problem.Input;
    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.Dal.Repositories.Problem;

    public class ProblemLogService : IProblemLogService
    {
        private readonly IProblemLogRepository problemLogRepository;
        private readonly IProblemRepository problemRepository;
        private readonly IProblemEMailLogRepository problemEMailLogRepository;

        public ProblemLogService(IProblemLogRepository problemLogRepository, IProblemEMailLogRepository problemEMailLogRepository, IProblemRepository problemRepository)
        {
            this.problemLogRepository = problemLogRepository;
            this.problemEMailLogRepository = problemEMailLogRepository;
            this.problemRepository = problemRepository;
        }

        public NewProblemLogDto GetProblemLog(int id)
        {
            return this.problemLogRepository.FindById(id);
        }

        public IList<ProblemLogOverview> GetProblemLogs(int problemId)
        {
            return this.problemLogRepository.FindByProblemId(problemId);
        }

        public void AddLog(NewProblemLogDto problemLog)
        {
            this.problemRepository.UpdateFinishedDate(problemLog.ProblemId, problemLog.FinishingDate);
            this.problemRepository.Commit();

            this.problemLogRepository.Add(problemLog);
            this.problemLogRepository.Commit();
        }

        public void DeleteLog(int id)
        {
            this.problemEMailLogRepository.DeleteByLogId(id);
            this.problemEMailLogRepository.Commit();

            this.problemLogRepository.Delete(id);
            this.problemLogRepository.Commit();
        }

        public void UpdateLog(NewProblemLogDto problemLog)
        {
            this.problemRepository.UpdateFinishedDate(problemLog.ProblemId, problemLog.FinishingDate);
            this.problemRepository.Commit();

            this.problemLogRepository.Update(problemLog);
            this.problemLogRepository.Commit();
        }
    }
}