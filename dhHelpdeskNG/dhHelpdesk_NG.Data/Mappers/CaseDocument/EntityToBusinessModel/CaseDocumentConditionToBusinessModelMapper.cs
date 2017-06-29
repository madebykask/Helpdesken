namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;    
    using DH.Helpdesk.Domain;

    public  class CaseDocumentConditionToBusinessModelMapper : IEntityToBusinessModelMapper<CaseDocumentConditionEntity, CaseDocumentConditionModel>
    {
        public CaseDocumentConditionModel Map(CaseDocumentConditionEntity entity)
        {
            return new CaseDocumentConditionModel
            {
                Id = entity.Id,
                Property_Name = entity.Property_Name,
                CaseDocumentConditionGUID = entity.CaseDocumentConditionGUID,
                ChangedDate = entity.ChangedDate,
                CaseDocument_Id = entity.CaseDocument_Id,
                CreatedDate = entity.CreatedDate,
                ChangedByUser_Id = entity.ChangedByUser_Id,
                CreatedByUser_Id = entity.CreatedByUser_Id,
                Status = entity.Status,
                Values = entity.Values,
                //Operator = entity.Operator
            };
        }
    }
}