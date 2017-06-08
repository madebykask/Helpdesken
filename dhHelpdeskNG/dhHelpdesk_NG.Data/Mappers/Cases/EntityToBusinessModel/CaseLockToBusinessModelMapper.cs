namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Case.CaseLock;    
    using DH.Helpdesk.Domain.Cases;

    public sealed class CaseLockToBusinessModelMapper : IEntityToBusinessModelMapper<CaseLockEntity, CaseLock>
    {
        public CaseLock Map(CaseLockEntity entity)
        {
            return new CaseLock(entity.Case_Id, 
                                entity.User_Id, 
                                entity.LockGUID, 
                                entity.BrowserSession, 
                                entity.CreatedTime, 
                                entity.ExtendedTime,
                                entity.User,
                                entity.ActiveTab);                        
        }
    }
}