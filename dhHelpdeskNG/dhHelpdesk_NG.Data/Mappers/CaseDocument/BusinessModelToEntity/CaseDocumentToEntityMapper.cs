using System.Collections.Generic;
using System.Linq;

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

            if (businessModel.CaseDocumentParagraphs != null && businessModel.CaseDocumentParagraphs.Any())
            {
                var mapper = new CaseDocumentParagraphToEntityMapper();
                entity.CaseDocumentParagraphs = new List<CaseDocumentParagraphEntity>();
                foreach (var paragModel in businessModel.CaseDocumentParagraphs)
                {
                    var paragraph = new CaseDocumentParagraphEntity();
                    mapper.Map(paragModel, paragraph);
                    entity.CaseDocumentParagraphs.Add(paragraph);
                }
            }

            entity.CaseDocumentTemplate = businessModel.CaseDocumentTemplate;
            
        }
    }
}