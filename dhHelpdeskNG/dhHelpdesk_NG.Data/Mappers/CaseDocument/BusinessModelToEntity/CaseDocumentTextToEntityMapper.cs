namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Domain;

    public sealed class CaseDocumentTextToEntityMapper : IBusinessModelToEntityMapper<CaseDocumentTextModel, CaseDocumentTextEntity>
    {
        public void Map(CaseDocumentTextModel businessModel, CaseDocumentTextEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            entity.Id = businessModel.Id;
            entity.CaseDocumentParagraph_Id = businessModel.CaseDocumentParagraph_Id;
            entity.Name = businessModel.Name;
            entity.Description = businessModel.Description;
            entity.Text = businessModel.Text;
            entity.Headline = businessModel.Headline;
            entity.SortOrder = businessModel.SortOrder;

        }
    }
}