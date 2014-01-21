namespace dhHelpdesk_NG.Data.Repositories.Problem
{
    using System;
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public interface IProblemRepository : INewRepository<NewProblemDto, NewProblemDto>
    {
        void UpdateFinishedDate(int problemId, DateTime? time);

        ProblemOverview FindById(int problemId);

        List<ProblemOverview> FindByCustomerId(int customerId);

        List<ProblemOverview> FindByCustomerIdAndStatus(int customerId, EntityStatus entityStatus);
    }
}