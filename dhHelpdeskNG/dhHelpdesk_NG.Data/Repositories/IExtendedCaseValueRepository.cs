namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;

    public interface IExtendedCaseValueRepository : IRepository<ExtendedCaseValueEntity>
    {
       ExtendedCaseValueEntity GetExtendedCaseValue(int extendedCaseDataId, string fieldId);
    }
}