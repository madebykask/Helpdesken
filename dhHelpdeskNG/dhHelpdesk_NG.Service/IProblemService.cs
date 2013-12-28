namespace dhHelpdesk_NG.Service
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public interface IProblemService
    {
        void AddProblem(NewProblemDto problem);

        void DeleteProblem(int id);

        void UpdateProblem(NewProblemDto problem);

        ProblemOverview GetProblem(int id);

        IList<ProblemOverview> GetCustomerProblems(int customerId);

        IList<ProblemOverview> GetCustomerProblems(int customerId, EntityStatus show);
    }
}