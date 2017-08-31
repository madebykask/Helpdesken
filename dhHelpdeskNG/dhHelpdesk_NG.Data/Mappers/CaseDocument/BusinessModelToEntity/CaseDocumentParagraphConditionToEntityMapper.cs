namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Domain;

    public sealed class CaseDocumentParagraphConditionToEntityMapper : IBusinessModelToEntityMapper<CaseDocumentParagraphConditionModel, CaseDocumentParagraphConditionEntity>
    {
        public void Map(CaseDocumentParagraphConditionModel businessModel, CaseDocumentParagraphConditionEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            entity.Id = businessModel.Id;
            entity.Property_Name = businessModel.Property_Name;
            entity.CaseDocumentParagraphConditionGUID = businessModel.CaseDocumentParagraphConditionGUID;
            entity.CaseDocumentParagraph_Id = businessModel.CaseDocumentParagraph_Id;
            entity.ChangedDate = businessModel.ChangedDate;
            entity.ChangedByUser_Id = businessModel.ChangedByUser_Id;
            entity.CreatedDate = businessModel.CreatedDate;
            entity.CreatedByUser_Id = businessModel.CreatedByUser_Id;
            entity.Status = businessModel.Status;
            entity.Values = businessModel.Values;
            entity.Description = businessModel.Description;
            entity.Operator = businessModel.Operator;
        }
    }
}