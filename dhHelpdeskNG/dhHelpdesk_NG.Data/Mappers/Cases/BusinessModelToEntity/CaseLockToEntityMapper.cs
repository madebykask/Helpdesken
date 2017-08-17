namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Case.CaseLock;    
    using DH.Helpdesk.Domain.Cases;    

    public sealed class CaseLockToEntityMapper : IBusinessModelToEntityMapper<CaseLock, CaseLockEntity>
    {
        public void Map(CaseLock businessModel, CaseLockEntity entity)
        {
            if (entity == null)
            {
                return;
            }
            entity.Case_Id = businessModel.CaseId;
            entity.User_Id = businessModel.UserId;
            entity.LockGUID = businessModel.LockGUID;
            entity.BrowserSession = businessModel.BrowserSession ?? string.Empty;
            entity.CreatedTime = businessModel.CreatedTime;
            entity.ExtendedTime = businessModel.ExtendedTime;
            entity.ActiveTab = businessModel.ActiveTab;
        }
    }
}