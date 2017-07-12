namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;    
    using DH.Helpdesk.Domain;

    public  class CaseDocumentParagraphToBusinessModelMapper : IEntityToBusinessModelMapper<CaseDocumentParagraphEntity, CaseDocumentParagraphModel>
    {
        public CaseDocumentParagraphModel Map(CaseDocumentParagraphEntity entity)
        {
            return new CaseDocumentParagraphModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                ParagraphType = entity.ParagraphType,
                CaseDocumentTexts = entity.CaseDocumentTexts
            };
        }
    }
}