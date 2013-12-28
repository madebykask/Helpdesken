namespace dhHelpdesk_NG.Data.Repositories.Problem
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public interface IProblemRepository : IRepository<Problem>
    {
        void Add(NewProblemDto newProblem);

        void Delete(int problemId);
        
        void Update(NewProblemDto existingProblem);

        ProblemOverview FindById(int problemId);

        List<ProblemOverview> FindByCustomerId(int customerId);

        List<ProblemOverview> FindByCustomerIdAndStatus(int customerId, EntityStatus entityStatus);
    }
}