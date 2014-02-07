namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Problem.Input;
    using DH.Helpdesk.BusinessData.Models.Problem.Output;

    public interface IProblemLogService
    {
        NewProblemLogDto GetProblemLog(int id);

        IList<ProblemLogOverview> GetProblemLogs(int problemId);

        void AddLog(NewProblemLogDto problemLog);

        void DeleteLog(int id);

        void UpdateLog(NewProblemLogDto problemLog);
    }
}