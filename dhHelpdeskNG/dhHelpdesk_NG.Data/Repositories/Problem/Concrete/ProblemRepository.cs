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

    public class ProblemRepository : Repository<Problem, NewProblemDto, NewProblemDto>, IProblemRepository
    {
        public ProblemRepository(
            IDatabaseFactory databaseFactory,
            IBusinessModelToEntityMapper<NewProblemDto, Problem> newModelMapper,
            IEntityChangerFromBusinessModel<NewProblemDto, Problem> updatedModelMapper)
            : base(databaseFactory, newModelMapper, updatedModelMapper)
        {
        }

        public static ProblemOverview MapProblem(Problem problem)
        {
            return new ProblemOverview
                       {
                           Id = problem.Id,
                           Name = problem.Name,
                           Description = problem.Description,
                           ProblemNumber = problem.ProblemNumber,
                           ResponsibleUserId = problem.ResponsibleUser == null ? null : (int?)problem.ResponsibleUser.Id,
                           ResponsibleUserName = problem.ResponsibleUser == null ? null : problem.ResponsibleUser.FirstName,
                           InventoryNumber = problem.InventoryNumber,
                           ShowOnStartPage = problem.ShowOnStartPage == 1,
                           FinishingDate = problem.FinishingDate,
                           IsExistConnectedCases = problem.Cases.Any()
                       };
        }

        public ProblemOverview FindById(int problemId)
        {
            var problem = this.DbContext.Problems.Find(problemId);
            var problemOverview = MapProblem(problem);

            return problemOverview;
        }

        public List<ProblemOverview> FindByCustomerId(int customerId)
        {
            var propblemOverviews = this.DbContext.Problems.Where(x => x.Customer_Id == customerId)
                                                           .OrderBy(x => x.Name)
                                                           .Select(MapProblem)
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
                                        .Select(MapProblem)
                                        .ToList();
            return propblemOverviews;
        }
    }
}
