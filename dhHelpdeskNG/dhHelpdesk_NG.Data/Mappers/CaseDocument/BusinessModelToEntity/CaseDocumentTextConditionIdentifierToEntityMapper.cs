namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Domain;

    public sealed class CaseDocumentTextConditionIdentifierToEntityMapper : IBusinessModelToEntityMapper<CaseDocumentTextConditionIdentifierModel, CaseDocumentTextConditionIdentifierEntity>
    {
        public void Map(CaseDocumentTextConditionIdentifierModel businessModel, CaseDocumentTextConditionIdentifierEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            entity.Id = businessModel.Id;
            entity.ExtendedCaseFormId = businessModel.ExtendedCaseFormId;
            entity.Identifier = businessModel.Identifier;
            entity.PropertyName = businessModel.PropertyName;
            entity.DisplayName = businessModel.DisplayName;
        }
    }
}