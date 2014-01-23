namespace dhHelpdesk_NG.Data.Repositories.Problem.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.Data.Dal.Mappers;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Problems;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public class ProblemLogRepository : Repository, IProblemLogRepository
    {
        private readonly IBusinessModelToEntityMapper<NewProblemLogDto, ProblemLog> newModelMapper;

        private readonly IEntityChangerFromBusinessModel<NewProblemLogDto, ProblemLog> updatedModelMapper;

        private readonly IEntityToBusinessModelMapper<ProblemLog, ProblemLogOverview> overviewMapper;

        private readonly IEntityToBusinessModelMapper<ProblemLog, NewProblemLogDto> dtoMapper;

        public ProblemLogRepository(
            IDatabaseFactory databaseFactory,
            IBusinessModelToEntityMapper<NewProblemLogDto, ProblemLog> newModelMapper,
            IEntityChangerFromBusinessModel<NewProblemLogDto, ProblemLog> updatedModelMapper,
            IEntityToBusinessModelMapper<ProblemLog, ProblemLogOverview> overviewMapper,
            IEntityToBusinessModelMapper<ProblemLog, NewProblemLogDto> dtoMapper)
            : base(databaseFactory)
        {
            this.newModelMapper = newModelMapper;
            this.updatedModelMapper = updatedModelMapper;
            this.overviewMapper = overviewMapper;
            this.dtoMapper = dtoMapper;
        }

        public virtual void Add(NewProblemLogDto businessModel)
        {
            var entity = this.newModelMapper.Map(businessModel);
            this.DbContext.ProblemLogs.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public virtual void Delete(int id)
        {
            var entity = this.DbContext.ProblemLogs.Find(id);
            this.DbContext.ProblemLogs.Remove(entity);
        }

        public void Update(NewProblemLogDto businessModel)
        {
            var entity = this.DbContext.ProblemLogs.Find(businessModel.Id);
            this.updatedModelMapper.Map(businessModel, entity);
        }

        public void DeleteByProblemId(int problemId)
        {
            var problemLogs = this.DbContext.ProblemLogs.Where(x => x.Problem_Id == problemId).ToList();
            problemLogs.ForEach(x => this.DbContext.ProblemLogs.Remove(x));
        }

        public NewProblemLogDto FindById(int problemLogId)
        {
            var problemLog = this.DbContext.ProblemLogs.Find(problemLogId);
            var problemLogOverview = this.dtoMapper.Map(problemLog);

            return problemLogOverview;
        }

        public List<ProblemLogOverview> FindByProblemId(int problemId)
        {
            var problemLogs = this.DbContext.ProblemLogs.Where(x => x.Problem_Id == problemId)
                                                        .OrderBy(x => x.CreatedDate)
                                                        .Select(this.overviewMapper.Map)
                                                        .ToList();
            return problemLogs;
        }
    }
}