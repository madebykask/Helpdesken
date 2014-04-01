namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Problem.Input;
    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.Dal.Enums;

    public interface IProblemService
    {
        void AddProblem(NewProblemDto problem);

        void AddProblem(NewProblemDto problem, NewProblemLogDto problemLogDto);

        void AddProblem(NewProblemDto problem, IList<NewProblemLogDto> problemLogDtos);

        void DeleteProblem(int id);

        void UpdateProblem(NewProblemDto problem);

        void ActivateProblem(int id);

        ProblemOverview GetProblem(int id);

        IList<ProblemOverview> GetCustomerProblems(int customerId);

        IList<ProblemOverview> GetCustomerProblems(int customerId, EntityStatus show);

        IEnumerable<ProblemInfoOverview> GetProblemOverviews(int[] customers, int? count = null);
    }
}