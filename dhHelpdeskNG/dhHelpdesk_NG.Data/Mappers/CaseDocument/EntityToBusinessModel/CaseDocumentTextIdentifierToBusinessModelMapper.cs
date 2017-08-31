namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Domain;

    public class CaseDocumentTextIdentifierToBusinessModelMapper : IEntityToBusinessModelMapper<CaseDocumentTextIdentifierEntity, CaseDocumentTextIdentifierModel>
    {
        public CaseDocumentTextIdentifierModel Map(CaseDocumentTextIdentifierEntity entity)
        {
            return new CaseDocumentTextIdentifierModel
            {
                Id = entity.Id,
                ExtendedCaseFormId = entity.ExtendedCaseFormId,
                Identifier = entity.Identifier,
                PropertyName = entity.PropertyName,
                DisplayName = entity.DisplayName
            };
        }
    }
}