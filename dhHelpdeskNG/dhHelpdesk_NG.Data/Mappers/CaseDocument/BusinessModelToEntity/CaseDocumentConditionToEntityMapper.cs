namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Domain;

    public sealed class CaseDocumentConditionToEntityMapper : IBusinessModelToEntityMapper<CaseDocumentConditionModel, CaseDocumentConditionEntity>
    {
        public void Map(CaseDocumentConditionModel businessModel, CaseDocumentConditionEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            entity.Id = businessModel.Id;
            entity.Property_Name = businessModel.Property_Name;
            entity.CaseDocumentConditionGUID = businessModel.CaseDocumentConditionGUID;
            entity.CaseDocument_Id = businessModel.CaseDocument_Id;
            entity.ChangedDate = businessModel.ChangedDate;
            entity.ChangedByUser_Id = businessModel.ChangedByUser_Id;
            entity.CreatedDate = businessModel.CreatedDate;
            entity.CreatedByUser_Id = businessModel.CreatedByUser_Id;
            entity.Status = businessModel.Status;
            entity.Values = businessModel.Values;
            entity.Description = businessModel.Description;
            //entity.Operator = businessModel.Operator;
        }
    }
}