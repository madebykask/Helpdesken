namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;    
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Cases;
    using BusinessData.Models.ExtendedCase;

    public interface IWorkFlowRepository : IRepository<WorkFlowEntity>
    {
        IList<WorkFlowModel> GetWorkFlowItems(int caseId);
    }
}