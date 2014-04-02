using DH.Helpdesk.BusinessData.Models.Users.Input;
using DH.Helpdesk.Domain.Users;

namespace DH.Helpdesk.Dal.Mappers.Users.BusinessModelToEntity
{
    public class UpdatedUserModuleToUserModuleEntityMapper : IBusinessModelToEntityMapper<UserModule, UserModuleEntity>
    {
        public void Map(UserModule businessModel, UserModuleEntity entity)
        {
            entity.User_Id = businessModel.User_Id;
            entity.Module_Id = businessModel.Module_Id;
            entity.Position = businessModel.Position;
            entity.isVisible = businessModel.isVisible;
            entity.NumberOfRows = businessModel.NumberOfRows;
        }
    }
}