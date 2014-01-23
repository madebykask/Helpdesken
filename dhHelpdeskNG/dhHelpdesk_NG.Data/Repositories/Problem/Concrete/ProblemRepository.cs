namespace dhHelpdesk_NG.Data.Repositories.Problem.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.Data.Dal.Mappers;
    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Problems;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public class ProblemRepository : Repository, IProblemRepository
    {
        private readonly IBusinessModelToEntityMapper<NewProblemDto, Problem> newModelMapper;

        private readonly IEntityChangerFromBusinessModel<NewProblemDto, Problem> updatedModelMapper;

        private readonly IEntityToBusinessModelMapper<Problem, ProblemOverview> overviewMapper;

        public ProblemRepository(
            IDatabaseFactory databaseFactory,
            IBusinessModelToEntityMapper<NewProblemDto, Problem> newModelMapper,
            IEntityChangerFromBusinessModel<NewProblemDto, Problem> updatedModelMapper,
            IEntityToBusinessModelMapper<Problem, ProblemOverview> overviewMapper)
            : base(databaseFactory)
        {
            this.newModelMapper = newModelMapper;
            this.updatedModelMapper = updatedModelMapper;
            this.overviewMapper = overviewMapper;
        }

        public virtual void Add(NewProblemDto businessModel)
        {
            var entity = this.newModelMapper.Map(businessModel);
            this.DbContext.Problems.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public virtual void Delete(int id)
        {
            var entity = this.DbContext.Problems.Find(id);
            this.DbContext.Problems.Remove(entity);
        }

        public void Update(NewProblemDto businessModel)
        {
            var entity = this.DbContext.Problems.Find(businessModel.Id);
            this.updatedModelMapper.Map(businessModel, entity);
        }

        public ProblemOverview FindById(int problemId)
        {
            var problem = this.DbContext.Problems.Find(problemId);
            var problemOverview = this.overviewMapper.Map(problem);

            return problemOverview;
        }

        public List<ProblemOverview> FindByCustomerId(int customerId)
        {
            var propblemOverviews = this.DbContext.Problems.Where(x => x.Customer_Id == customerId)
                                                           .OrderBy(x => x.Name)
                                                           .Select(this.overviewMapper.Map)
                                                           .ToList();
            return propblemOverviews;
        }

        public void UpdateFinishedDate(int problemId, DateTime? time)
        {
            var problem = this.DbContext.Problems.Find(problemId);
            problem.FinishingDate = time;
        }

        public List<ProblemOverview> FindByCustomerIdAndStatus(int customerId, EntityStatus entityStatus)
        {
            var problems = this.DbContext.Problems.Where(x => x.Customer_Id == customerId);

            switch (entityStatus)
            {
                case EntityStatus.Active:
                    problems = problems.Where(x => x.FinishingDate == null);
                    break;
                case EntityStatus.Inactive:
                    problems = problems.Where(x => x.FinishingDate != null);
                    break;
            }

            var propblemOverviews = problems
                                        .OrderBy(x => x.Name)
                                        .Select(this.overviewMapper.Map)
                                        .ToList();
            return propblemOverviews;
        }
    }
}
