namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Domain;

    public class CaseDocumentTextConditionIdentifierToBusinessModelMapper : IEntityToBusinessModelMapper<CaseDocumentTextConditionIdentifierEntity, CaseDocumentTextConditionIdentifierModel>
    {
        public CaseDocumentTextConditionIdentifierModel Map(CaseDocumentTextConditionIdentifierEntity entity)
        {
            return new CaseDocumentTextConditionIdentifierModel
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