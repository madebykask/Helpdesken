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

        IList<ProblemOverview> GetCustomerProblems(int customerId, bool checkCaseRelation = true);

        IList<ProblemOverview> GetCustomerProblems(int customerId, EntityStatus show);

        /// <summary>
        /// The get problem overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="forStartPage">
        /// The for start page.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<ProblemInfoOverview> GetProblemOverviews(int[] customers, int? count, bool forStartPage);

        int GetCaseHistoryId(int caseId, int problemId);
    }
}