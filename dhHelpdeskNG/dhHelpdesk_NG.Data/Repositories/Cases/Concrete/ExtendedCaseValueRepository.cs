using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.ExtendedCaseEntity;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    public sealed class ExtendedCaseValueRepository : RepositoryBase<ExtendedCaseValueEntity>, IExtendedCaseValueRepository
    { 
        public ExtendedCaseValueRepository(IDatabaseFactory databaseFactory) 
            : base(databaseFactory)
        {
        }

        public IList<ExtendedCaseValueEntity> GetExtendedCaseValues(int extendedCaseDataId)
        {
            return Table.Where(c => c.ExtendedCaseDataId == extendedCaseDataId).ToList();
        }

        public ExtendedCaseValueEntity GetExtendedCaseValue(int extendedCaseDataId, string fieldId)
        {
            return Table
                  .Where(c => c.ExtendedCaseDataId == extendedCaseDataId && c.FieldId.ToLower() == fieldId.ToLower()) 
                  .Distinct()
                  .FirstOrDefault();
        }
    }
}