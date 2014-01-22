namespace dhHelpdesk_NG.Data.Repositories.Problem
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public interface IProblemLogRepository : INewRepository
    {
        void Add(NewProblemLogDto businessModel);

        void Delete(int id);

        void Update(NewProblemLogDto businessModel);

        void DeleteByProblemId(int problemId);

        NewProblemLogDto FindById(int problemLogId);

        List<ProblemLogOverview> FindByProblemId(int problemId);
    }
}