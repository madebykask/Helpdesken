namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Problem.Input;
    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Problem;

    public class ProblemService : IProblemService
    {
        private readonly IProblemRepository problemRepository;
        private readonly IProblemLogRepository problemLogRepository;
        private readonly IProblemEMailLogRepository problemEMailLogRepository;
        private readonly ICaseHistoryRepository caseHistoryRepository;
        private readonly ICaseRepository caseRepository;

        public ProblemService(IProblemRepository problemRepository, IProblemLogRepository problemLogRepository, IProblemEMailLogRepository problemEMailLogRepository, ICaseHistoryRepository caseHistoryRepository, ICaseRepository caseRepository)
        {
            this.problemRepository = problemRepository;
            this.problemLogRepository = problemLogRepository;
            this.problemEMailLogRepository = problemEMailLogRepository;
            this.caseHistoryRepository = caseHistoryRepository;
            this.caseRepository = caseRepository;
        }

        public ProblemOverview GetProblem(int id)
        {
            return this.problemRepository.FindById(id);
        }

        public IList<ProblemOverview> GetCustomerProblems(int customerId)
        {
            return this.problemRepository.FindByCustomerId(customerId).OrderBy(x => x.ProblemNumber).ToList();
        }

        public IList<ProblemOverview> GetCustomerProblems(int customerId, EntityStatus show)
        {
            return this.problemRepository.FindByCustomerIdAndStatus(customerId, show).OrderBy(x => x.ProblemNumber).ToList();
        }

        public void AddProblem(NewProblemDto problem)
        {
            this.problemRepository.Add(problem);
            this.problemRepository.Commit();
        }

        public void AddProblem(NewProblemDto problem, NewProblemLogDto problemLogDto)
        {
            problem.FinishingDate = problemLogDto.FinishingDate;

            this.problemRepository.Add(problem);
            this.problemRepository.Commit();

            problemLogDto.ProblemId = problem.Id;

            this.problemLogRepository.Add(problemLogDto);
            this.problemLogRepository.Commit();
        }

        public void AddProblem(NewProblemDto problem, IList<NewProblemLogDto> problemLogDtos)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteProblem(int id)
        {
            // ToDo Artem: do not use implementation specific names in interfaces.
            this.caseHistoryRepository.SetNullProblemByProblemId(id);
            this.caseHistoryRepository.Commit();

            this.caseRepository.SetNullProblemByProblemId(id);
            this.caseRepository.Commit();

            this.problemEMailLogRepository.DeleteByProblemId(id);
            this.problemEMailLogRepository.Commit();

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

        public void ActivateProblem(int id)
        {
            this.problemRepository.UpdateFinishedDate(id, null);
            this.problemRepository.Commit();
        }
    }
}
