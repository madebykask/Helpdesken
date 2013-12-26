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

        ProblemOverview FindById(int id);

        IList<ProblemOverview> FindByCustomerId(int customerId);

        IList<ProblemOverview> FindByCustomerIdAndStatus(int customerId, EntityStatus show);
    }
}