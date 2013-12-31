namespace dhHelpdesk_NG.Service.Concrete
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Repositories.Problem;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public class ProblemLogService : IProblemLogService
    {
        private readonly IProblemLogRepository problemLogRepository;

        public ProblemLogService(IProblemLogRepository problemLogRepository)
        {
            this.problemLogRepository = problemLogRepository;
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
            this.problemLogRepository.Add(problemLog);
            this.problemLogRepository.Commit();
        }

        public void DeleteLog(int id)
        {
            this.problemLogRepository.Delete(id);
            this.problemLogRepository.Commit();
        }

        public void UpdateLog(NewProblemLogDto problemLog)
        {
            this.problemLogRepository.Update(problemLog);
            this.problemLogRepository.Commit();
        }
    }
}