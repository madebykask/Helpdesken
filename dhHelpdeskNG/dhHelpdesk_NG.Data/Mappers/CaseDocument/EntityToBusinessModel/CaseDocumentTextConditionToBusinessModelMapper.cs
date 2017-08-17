namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;    
    using DH.Helpdesk.Domain;

    public  class CaseDocumentTextConditionToBusinessModelMapper : IEntityToBusinessModelMapper<CaseDocumentTextConditionEntity, CaseDocumentTextConditionModel>
    {
        public CaseDocumentTextConditionModel Map(CaseDocumentTextConditionEntity entity)
        {
            return new CaseDocumentTextConditionModel
            {
                Id = entity.Id,
                Property_Name = entity.Property_Name,
                CaseDocumentTextConditionGUID = entity.CaseDocumentTextConditionGUID,
                ChangedDate = entity.ChangedDate,
                CaseDocumentText_Id = entity.CaseDocumentText_Id,
                CreatedDate = entity.CreatedDate,
                ChangedByUser_Id = entity.ChangedByUser_Id,
                CreatedByUser_Id = entity.CreatedByUser_Id,
                Status = entity.Status,
                Values = entity.Values,
                Operator = entity.Operator
            };
        }
    }
}