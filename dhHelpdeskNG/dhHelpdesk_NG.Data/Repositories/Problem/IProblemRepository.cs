namespace DH.Helpdesk.Dal.Repositories.Problem
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Problem.Input;
    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Enums;

    public interface IProblemRepository : INewRepository
    {
        void Add(NewProblemDto businessModel);

        void Delete(int id);

        void Update(NewProblemDto businessModel);

        void UpdateFinishedDate(int problemId, DateTime? time);

        ProblemOverview FindById(int problemId);

        List<ProblemOverview> FindByCustomerId(int customerId);

        List<ProblemOverview> FindByCustomerIdAndStatus(int customerId, EntityStatus entityStatus);

        IEnumerable<ProblemInfoOverview> GetProblemOverviews(int[] customers);
    }
}