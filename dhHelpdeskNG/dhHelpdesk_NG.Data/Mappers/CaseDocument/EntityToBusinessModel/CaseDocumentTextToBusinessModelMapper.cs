namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;    
    using DH.Helpdesk.Domain;

    public  class CaseDocumentTextToBusinessModelMapper : IEntityToBusinessModelMapper<CaseDocumentTextEntity, CaseDocumentTextModel>
    {
        public CaseDocumentTextModel Map(CaseDocumentTextEntity entity)
        {
            return new CaseDocumentTextModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CaseDocumentParagraph_Id = entity.CaseDocumentParagraph_Id,
                Text = entity.Text,
                Headline = entity.Headline,
                SortOrder = entity.SortOrder
            };
        }
    }
}