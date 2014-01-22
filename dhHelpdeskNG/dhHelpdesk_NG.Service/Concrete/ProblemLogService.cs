namespace dhHelpdesk_NG.Service.Concrete
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Repositories.Problem;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

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

            this.problemLogRepository.DeleteByProblemId(id);
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