namespace dhHelpdesk_NG.Data.Repositories.Problem.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Problems;
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
                           ResponsibleUserId = problem.ResponsibleUser == null ? null : (int?)problem.ResponsibleUser.Id,
                           ResponsibleUserName = problem.ResponsibleUser == null ? null : problem.ResponsibleUser.FirstName,
                           InventoryNumber = problem.InventoryNumber,
                           ShowOnStartPage = problem.ShowOnStartPage == 1,
                           FinishingDate = problem.FinishingDate
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

        public void Add(NewProblemDto newProblem)
        {
            var problem = MapProblem(newProblem);
            problem.ProblemNumber = this.DataContext.Problems.Max(x => x.ProblemNumber) + 1;
            this.Add(problem);
            this.InitializeAfterCommit(newProblem, problem);
        }

        public void Delete(int problemId)
        {
            var problem = this.DataContext.Problems.Find(problemId);
            this.DataContext.Problems.Remove(problem);
        }

        public void Update(NewProblemDto existingProblem)
        {
            var problem = GetById(existingProblem.Id);
            problem.Name = existingProblem.Name;
            problem.Description = existingProblem.Description;
            problem.ResponsibleUser_Id = existingProblem.ResponsibleUserId;
            problem.InventoryNumber = existingProblem.InventoryNumber;
            problem.ShowOnStartPage = existingProblem.ShowOnStartPage ? 1 : 0;
            problem.ChangedDate = DateTime.Now;
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
