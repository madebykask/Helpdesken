using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.ExtendedCaseEntity;

namespace DH.Helpdesk.Dal.Repositories
{
    public interface IExtendedCaseValueRepository : IRepository<ExtendedCaseValueEntity>
    {
       ExtendedCaseValueEntity GetExtendedCaseValue(int extendedCaseDataId, string fieldId);
    }
}