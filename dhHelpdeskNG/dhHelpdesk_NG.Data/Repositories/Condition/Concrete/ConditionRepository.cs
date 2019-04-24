using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Condition;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.Repositories.Condition.Concrete
{
    public sealed class ConditionRepository : RepositoryBase<ConditionEntity>, IConditionRepository
    {
        private readonly IEntityToBusinessModelMapper<ConditionEntity, ConditionModel> _conditionModelMapper;

        public ConditionRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<ConditionEntity, ConditionModel> conditionModelMapper)
            : base(databaseFactory)
        {
            _conditionModelMapper = conditionModelMapper;
        }

        public IList<ConditionModel> GetConditions(int parentId, int conditionTypeId)
        {
            var entities = Table
                   .Where(c => c.Parent_Id == parentId && c.Status != 0 && c.ConditionType_Id == conditionTypeId)
                   .ToList();

            return entities.Select(_conditionModelMapper.Map).ToList();
        }
    }
}
