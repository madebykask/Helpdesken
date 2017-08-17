namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Domain;

    public sealed class CaseDocumentParagraphToEntityMapper : IBusinessModelToEntityMapper<CaseDocumentParagraphModel, CaseDocumentParagraphEntity>
    {
        public void Map(CaseDocumentParagraphModel businessModel, CaseDocumentParagraphEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            entity.Id = businessModel.Id;
            entity.Name = businessModel.Name;
            entity.Description = businessModel.Description;
            entity.ParagraphType = businessModel.ParagraphType;
            entity.CaseDocumentTexts = businessModel.CaseDocumentTexts;
        }
    }
}