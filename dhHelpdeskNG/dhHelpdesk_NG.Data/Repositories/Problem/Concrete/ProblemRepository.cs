namespace dhHelpdesk_NG.Data.Repositories.Problem.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public class ProblemRepository : RepositoryBase<Problem>, IProblemRepository
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
                           ResponsibleUser = problem.ResponsibleUser == null ? string.Empty : problem.ResponsibleUser.FirstName
                       };
        }

        public void Add(NewProblemDto newProblem)
        {
        }

        public void DeleteById(int problemId)
        {
        }

        public void Save(NewProblemDto newProblem)
        {
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
                    problems = problems.Where(x => x.FinishingDate != null);
                    break;
                case EntityStatus.Inactive:
                    problems = problems.Where(x => x.FinishingDate == null);
                    break;
            }

            var propblemOverviews = problems
                                        .OrderBy(x => x.Name)
                                        .Select(MapProblem)
                                        .ToList();
            return propblemOverviews;
        }

        public void Update(NewProblemDto existingProblem)
        {
        }
    }
}
