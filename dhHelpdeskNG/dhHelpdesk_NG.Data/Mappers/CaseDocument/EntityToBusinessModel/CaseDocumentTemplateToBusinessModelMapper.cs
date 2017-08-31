namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;    
    using DH.Helpdesk.Domain;

    public  class CaseDocumentTemplateToBusinessModelMapper : IEntityToBusinessModelMapper<CaseDocumentTemplateEntity, CaseDocumentTemplateModel>
    {
        public CaseDocumentTemplateModel Map(CaseDocumentTemplateEntity entity)
        {
            return new CaseDocumentTemplateModel
            {
                Id = entity.Id,
                Name = entity.Name,
                PageNumbersUse = entity.PageNumbersUse,
                CaseDocumentTemplateGUID = entity.CaseDocumentTemplateGUID,
                MarginTop = entity.MarginTop,
                MarginBottom = entity.MarginBottom,
                MarginLeft = entity.MarginLeft,
                MarginRight = entity.MarginRight,
                FooterHeight = entity.FooterHeight,
                HeaderHeight = entity.HeaderHeight,
            };
        }
    }
}