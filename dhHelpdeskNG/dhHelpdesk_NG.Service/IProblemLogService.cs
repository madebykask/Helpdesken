namespace dhHelpdesk_NG.Service
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public interface IProblemLogService
    {
        NewProblemLogDto GetProblemLog(int id);

        IList<ProblemLogOverview> GetProblemLogs(int problemId);

        void AddLog(NewProblemLogDto problemLog);

        void DeleteLog(int id);

        void UpdateLog(NewProblemLogDto problemLog);
    }
}