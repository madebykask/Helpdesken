namespace dhHelpdesk_NG.Data.Repositories.Problem
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public interface IProblemLogRepository : IRepository<ProblemLog>
    {
        void Add(NewProblemLogDto newProblemLog);

        void Delete(int problemLogId);

        void DeleteByProblemId(int problemId);

        void Update(NewProblemLogDto existingProblemLog);

        NewProblemLogDto FindById(int problemLogId);

        List<ProblemLogOverview> FindByProblemId(int problemId);
    }
}