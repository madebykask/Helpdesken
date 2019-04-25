using System.Collections.Generic;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain;
using DH.Helpdesk.BusinessData.Models.Condition;

namespace DH.Helpdesk.Dal.Repositories.Condition
{
    public interface IConditionRepository : IRepository<ConditionEntity>
    {
        IList<ConditionModel> GetConditions(int parentId, int conditionTypeId);
    }
}