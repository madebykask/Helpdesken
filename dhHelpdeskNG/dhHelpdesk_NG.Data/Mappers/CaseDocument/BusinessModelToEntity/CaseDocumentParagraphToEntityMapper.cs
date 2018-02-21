
using System.Collections.Generic;
using System.Linq;

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

            if (businessModel.CaseDocumentTexts != null && businessModel.CaseDocumentTexts.Any())
            {
                var mapper = new CaseDocumentTextToEntityMapper();
                entity.CaseDocumentTexts = new List<CaseDocumentTextEntity>();

                foreach (var textModel in businessModel.CaseDocumentTexts)
                {
                    var textEntity = new CaseDocumentTextEntity();
                    mapper.Map(textModel, textEntity);
                    entity.CaseDocumentTexts.Add(textEntity);
                }
            }
        }
    }
}