namespace DH.Helpdesk.Dal.Repositories.Condition
{
    using System.Collections.Generic;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.Condition;
    
    public interface IConditionRepository : IRepository<ConditionEntity>
    {
        IEnumerable<ConditionModel> GetConditions(int parent_Id);
    }
}