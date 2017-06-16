namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Case.Input;
    using DH.Helpdesk.Domain.Computers;

    public sealed class CaseNotifierToEntityMapper : IBusinessModelToEntityMapper<CaseNotifier, ComputerUser>
    {
        public void Map(CaseNotifier businessModel, ComputerUser entity)
        {
            if (entity == null)
            {
                return;
            }

            // We will not update userid, firstname and surname to the initiator
            //entity.UserId = businessModel.UserId;
            //entity.FirstName = businessModel.FirstName ?? string.Empty;
            //entity.SurName = businessModel.LastName ?? string.Empty;
            entity.Email = businessModel.Email ?? string.Empty;
            entity.Phone = businessModel.Phone ?? string.Empty;
            entity.Cellphone = businessModel.Cellphone ?? string.Empty;
            entity.Department_Id = businessModel.DepartmentId;
            entity.OU_Id = businessModel.OuId;
            entity.Location = businessModel.Place ?? string.Empty;
            entity.UserCode = businessModel.UserCode ?? string.Empty;
            entity.Customer_Id = businessModel.CustomerId;
            entity.LanguageId = businessModel.LanguageId;
        }
    }
}