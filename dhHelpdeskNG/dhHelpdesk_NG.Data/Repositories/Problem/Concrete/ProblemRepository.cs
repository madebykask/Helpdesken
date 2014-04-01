namespace DH.Helpdesk.Dal.Repositories.Problem.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Problem.Input;
    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Problems;

    public class ProblemRepository : Repository, IProblemRepository
    {
        private readonly INewBusinessModelToEntityMapper<NewProblemDto, Problem> newModelMapper;

        private readonly IBusinessModelToEntityMapper<NewProblemDto, Problem> updatedModelMapper;

        private readonly IEntityToBusinessModelMapper<Problem, ProblemOverview> overviewMapper;

        public ProblemRepository(
            IDatabaseFactory databaseFactory,
            INewBusinessModelToEntityMapper<NewProblemDto, Problem> newModelMapper,
            IBusinessModelToEntityMapper<NewProblemDto, Problem> updatedModelMapper,
            IEntityToBusinessModelMapper<Problem, ProblemOverview> overviewMapper)
            : base(databaseFactory)
        {
            this.newModelMapper = newModelMapper;
            this.updatedModelMapper = updatedModelMapper;
            this.overviewMapper = overviewMapper;
        }

        public virtual void Add(NewProblemDto businessModel)
        {
            var nextNumber = DbContext.Problems.Max(x => x.ProblemNumber) + 1;
            var entity = this.newModelMapper.Map(businessModel);
            entity.ProblemNumber = nextNumber;
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
            this.DbContext.Problems.Select(p => new ProblemOverview { Description = p.Description }).ToList();

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

        public IEnumerable<ProblemInfoOverview> GetProblemOverviews(int[] customers)
        {
            return DbContext.Problems
                .Where(p => customers.Contains(p.Customer_Id))
                .Select(p => new ProblemInfoOverview()
                {
                    Customer_Id = p.Customer_Id,
                    CreatedDate = p.CreatedDate,
                    Description = p.Description,
                    Name = p.Name,
                    ProblemNumber = p.ProblemNumber
                });
        }
    }
}
