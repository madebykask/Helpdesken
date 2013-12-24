namespace dhHelpdesk_NG.Service
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public interface IProblemService
    {
        DeleteMessage DeleteProblem(int id);

        void SaveProblem(NewProblemDto problem, out IDictionary<string, string> errors);

        ProblemOverview GetProblemOverview(int problemId);

        List<ProblemOverview> GetCustomerProblemOverviews(int customerId);

        List<ProblemOverview> GetCustomerProblemOverviews(int customerId, bool isActive);
    }
}