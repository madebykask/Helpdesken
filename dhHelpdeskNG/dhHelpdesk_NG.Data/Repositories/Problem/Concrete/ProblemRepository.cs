namespace dhHelpdesk_NG.Data.Repositories.Problem.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Problems;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public class ProblemRepository : RepositoryDecoratorBase<Problem, NewProblemDto>, IProblemRepository
    {
        public ProblemRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
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

        public static Problem MapProblem(NewProblemDto problem)
        {
            return new Problem
            {
                Id = problem.Id,
                Name = problem.Name,
                Description = problem.Description,
                ResponsibleUser_Id = problem.ResponsibleUserId,
                InventoryNumber = problem.InventoryNumber,
                ShowOnStartPage = problem.ShowOnStartPage ? 1 : 0,
                Customer_Id = problem.CustomerId,
                FinishingDate = problem.FinishingDate
            };
        }

        public override Problem MapFromDto(NewProblemDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.InventoryNumber))
            {
                dto.InventoryNumber = string.Empty;
            }

            return MapProblem(dto);
        }

        public ProblemOverview FindById(int problemId)
        {
            var problem = this.GetById(problemId);
            var problemOverview = MapProblem(problem);

            return problemOverview;
        }

        public List<ProblemOverview> FindByCustomerId(int customerId)
        {
            var propblemOverviews = this.GetMany(x => x.Customer_Id == customerId)
                                        .OrderBy(x => x.Name)
                                        .Select(MapProblem)
                                        .ToList();
            return propblemOverviews;
        }

        public List<ProblemOverview> FindByCustomerIdAndStatus(int customerId, EntityStatus entityStatus)
        {
            var problems = this.GetMany(x => x.Customer_Id == customerId);

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
