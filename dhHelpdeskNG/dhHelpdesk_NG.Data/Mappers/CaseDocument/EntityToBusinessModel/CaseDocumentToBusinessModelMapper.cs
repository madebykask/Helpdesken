using System;

namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;    
    using DH.Helpdesk.Domain;

    public  class CaseDocumentToBusinessModelMapper : IEntityToBusinessModelMapper<CaseDocumentEntity, CaseDocumentModel>
    {
        public CaseDocumentModel Map(CaseDocumentEntity entity)
        {
            return new CaseDocumentModel
            {
                Id = entity.Id,
                Name = entity.Name,
                CaseDocumentGUID = entity.CaseDocumentGUID,
                Customer_Id = entity.Customer_Id,
                Description = entity.Description,
                FileType = entity.FileType,
                SortOrder = entity.SortOrder,
                Status = entity.Status,
                CreatedDate = entity.CreatedDate,
                CreatedByUser_Id = entity.CreatedByUser_Id,
                ChangedDate = entity.ChangedDate,
                ChangedByUser_Id = entity.ChangedByUser_Id
            };
        }
    }
}