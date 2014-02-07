namespace DH.Helpdesk.Dal.Repositories.Problem
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Problem.Input;
    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.Dal.Dal;

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