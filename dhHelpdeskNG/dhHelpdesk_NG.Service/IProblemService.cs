namespace dhHelpdesk_NG.Service
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public interface IProblemService
    {
        DeleteMessage DeleteProblem(int id);

        void SaveProblem(NewProblemDto problem, out IDictionary<string, string> errors);

        ProblemOverview GetProblem(int id);

        IList<ProblemOverview> GetCustomerProblems(int customerId);

        IList<ProblemOverview> GetCustomerProblems(int customerId, EntityStatus show);
    }
}