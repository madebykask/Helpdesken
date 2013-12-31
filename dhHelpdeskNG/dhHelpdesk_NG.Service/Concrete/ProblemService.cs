namespace dhHelpdesk_NG.Service.Concrete
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Repositories.Problem;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public class ProblemService : IProblemService
    {
        private readonly IProblemRepository problemRepository;
        private readonly IProblemLogRepository problemLogRepository;

        public ProblemService(IProblemRepository problemRepository, IProblemLogRepository problemLogRepository)
        {
            this.problemRepository = problemRepository;
            this.problemLogRepository = problemLogRepository;
        }

        public ProblemOverview GetProblem(int id)
        {
            return this.problemRepository.FindById(id);
        }

        public IList<ProblemOverview> GetCustomerProblems(int customerId)
        {
            return this.problemRepository.FindByCustomerId(customerId);
        }

        public IList<ProblemOverview> GetCustomerProblems(int customerId, EntityStatus show)
        {
            return this.problemRepository.FindByCustomerIdAndStatus(customerId, show);
        }

        public void AddProblem(NewProblemDto problem)
        {
            this.problemRepository.Add(problem);
            this.problemRepository.Commit();
        }

        public void DeleteProblem(int id)
        {
            this.problemLogRepository.DeleteByProblemId(id);
            this.problemLogRepository.Commit();

            this.problemRepository.Delete(id);
            this.problemRepository.Commit();
        }

        public void UpdateProblem(NewProblemDto problem)
        {
            this.problemRepository.Update(problem);
            this.problemRepository.Commit();
        }
    }
}
