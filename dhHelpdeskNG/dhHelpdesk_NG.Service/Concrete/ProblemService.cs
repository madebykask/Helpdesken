namespace dhHelpdesk_NG.Service.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Transactions;

    using dhHelpdesk_NG.Data.Repositories.Problem;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public class ProblemService : IProblemService
    {
        private readonly IProblemRepository problemRepository;

        public ProblemService(IProblemRepository problemRepository)
        {
            this.problemRepository = problemRepository;
        }

        public ProblemOverview GetProblemOverview(int problemId)
        {
            return this.problemRepository.FindById(problemId);
        }

        public List<ProblemOverview> GetCustomerProblemOverviews(int customerId)
        {
            return this.problemRepository.FindByCustomerId(customerId);
        }

        public List<ProblemOverview> GetCustomerProblemOverviews(int customerId, bool isActive)
        {
            return this.problemRepository.FindByCustomerIdAndStatus(customerId, isActive);
        }

        public DeleteMessage DeleteProblem(int id)
        {
            var problem = this.problemRepository.GetById(id);

            if (problem != null)
            {
                try
                {
                    this.problemRepository.Delete(problem);
                    this.problemRepository.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public void SaveProblem(NewProblemDto problem, out IDictionary<string, string> errors)
        {
            using (var scope = new TransactionScope())
            {
                if (problem == null)
                {
                    throw new ArgumentNullException("problem");
                }

                errors = new Dictionary<string, string>();

                if (string.IsNullOrEmpty(problem.Name))
                {
                    errors.Add("Problem.Name", "Du måste ange ett problem");
                }

                if (string.IsNullOrEmpty(problem.InventoryNumber))
                {
                    errors.Add("Problem.InventoryNumber", "Du måste ange ett inventeringsnummer");
                }

                if (problem.Id == 0)
                {
                    this.problemRepository.Add(problem);
                }
                else
                {
                    this.problemRepository.Update(problem);
                }

                if (errors.Count == 0)
                {
                    scope.Complete();
                }
            }
        }
    }
}
