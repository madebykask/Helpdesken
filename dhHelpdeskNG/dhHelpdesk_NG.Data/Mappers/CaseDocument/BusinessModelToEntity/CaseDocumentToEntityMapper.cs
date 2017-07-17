namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Domain;

    public sealed class CaseDocumentToEntityMapper : IBusinessModelToEntityMapper<CaseDocumentModel, CaseDocumentEntity>
    {
        public void Map(CaseDocumentModel businessModel, CaseDocumentEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            entity.Id = businessModel.Id;
            entity.CaseDocumentGUID = businessModel.CaseDocumentGUID;
            entity.CaseDocumentTemplate_Id = businessModel.CaseDocumentTemplate_Id;
            entity.Name = businessModel.Name;
            entity.Description = businessModel.Description;
            entity.Customer_Id = businessModel.Customer_Id;
            entity.FileType = businessModel.FileType;
            entity.SortOrder = businessModel.SortOrder;
            entity.Status = businessModel.Status;
            entity.CreatedDate = businessModel.CreatedDate;
            entity.CreatedByUser_Id = businessModel.CreatedByUser_Id;
            entity.ChangedDate = businessModel.ChangedDate;
            entity.ChangedByUser_Id = businessModel.ChangedByUser_Id;
            entity.CaseDocumentParagraphs = businessModel.CaseDocumentParagraphs;
            entity.CaseDocumentTemplate = businessModel.CaseDocumentTemplate;
            
        }
    }
}