namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;    
    using DH.Helpdesk.Domain;

    public  class CaseDocumentParagraphConditionToBusinessModelMapper : IEntityToBusinessModelMapper<CaseDocumentParagraphConditionEntity, CaseDocumentParagraphConditionModel>
    {
        public CaseDocumentParagraphConditionModel Map(CaseDocumentParagraphConditionEntity entity)
        {
            return new CaseDocumentParagraphConditionModel
            {
                Id = entity.Id,
                Property_Name = entity.Property_Name,
                CaseDocumentParagraphConditionGUID = entity.CaseDocumentParagraphConditionGUID,
                ChangedDate = entity.ChangedDate,
                CaseDocumentParagraph_Id = entity.CaseDocumentParagraph_Id,
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