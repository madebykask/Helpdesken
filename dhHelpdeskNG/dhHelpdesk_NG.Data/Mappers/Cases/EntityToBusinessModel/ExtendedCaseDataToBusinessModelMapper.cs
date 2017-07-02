using System.Linq;

namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using BusinessData.Models.Case;
    using Domain.ExtendedCaseEntity;

    public  class ExtendedCaseDataToBusinessModelMapper : IEntityToBusinessModelMapper<ExtendedCaseDataEntity, ExtendedCaseDataModel>
    {
        public ExtendedCaseDataModel Map(ExtendedCaseDataEntity entity)
        {
            if (entity == null)
                return null;

            var model = new ExtendedCaseDataModel
            {
                Id = entity.Id,
                ExtendedCaseGuid = entity.ExtendedCaseGuid,
                ExtendedCaseFormId = entity.ExtendedCaseFormId,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            };

            if (entity.ExtendedCaseForm != null)
            {                
                var formModel = new ExtendedCaseFormModel
                {
                    Id = entity.ExtendedCaseForm.Id,
                    Name = entity.ExtendedCaseForm.Name
                };
                model.FormModel = formModel;
            }
            
            return model;
        }
    }
}