namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Domain;

    public sealed class CaseDocumentTextIdentifierToEntityMapper : IBusinessModelToEntityMapper<CaseDocumentTextIdentifierModel, CaseDocumentTextIdentifierEntity>
    {
        public void Map(CaseDocumentTextIdentifierModel businessModel, CaseDocumentTextIdentifierEntity entity)
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