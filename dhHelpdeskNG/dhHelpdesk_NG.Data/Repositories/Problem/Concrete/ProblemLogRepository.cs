namespace dhHelpdesk_NG.Data.Repositories.Problem.Concrete
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public class ProblemLogRepository : RepositoryBase<ProblemLog>, IProblemLogRepository
    {
        public ProblemLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(NewProblemLogDto newProblemLog)
        {
            throw new global::System.NotImplementedException();
        }

        public void Delete(int problemLogId)
        {
            throw new global::System.NotImplementedException();
        }

        public void Update(NewProblemLogDto existingProblemLog)
        {
            throw new global::System.NotImplementedException();
        }

        public ProblemLogOverview FindById(int problemLogId)
        {
            throw new global::System.NotImplementedException();
        }

        public List<ProblemLogOverview> FindByProblemId(int problemId)
        {
            throw new global::System.NotImplementedException();
        }
    }
}