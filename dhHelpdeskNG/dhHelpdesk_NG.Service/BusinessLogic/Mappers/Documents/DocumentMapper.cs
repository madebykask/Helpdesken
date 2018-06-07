namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Documents
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Document.Output;
    using DH.Helpdesk.Domain;

    public static class DocumentMapper
    {
        public static DocumentOverview[] MapToOverviews(this IQueryable<Document> query)
        {
            var entities = query.Select(d => new
                                        {
                                            d.Id,
                                            d.Name,
                                            d.CreatedDate,
                                            d.ChangedDate,
                                            d.Size,
                                            d.Customer
                                        }).ToArray();

            return entities.Select(d => new DocumentOverview
                                        {
                                            Id = d.Id,
                                            Name = d.Name,
                                            CreatedDate = d.CreatedDate,
                                            ChangedDate = d.ChangedDate,
                                            Size = d.Size,
                                            CustomerName = d.Customer.Name
                                        }).ToArray();
        }

        public static void MapToEntity(Document model, Document entity)
        {
            entity.File = model.File;
            entity.ChangedByUser_Id = model.ChangedByUser_Id;
            entity.CreatedByUser_Id = model.CreatedByUser_Id;
            entity.Customer_Id = model.Customer_Id;
            entity.DocumentCategory_Id = model.DocumentCategory_Id;
            entity.Size = model.Size;
            entity.ContentType = model.ContentType;
            entity.Description = model.Description;
            entity.FileName = model.FileName;
            entity.Name = model.Name;
            entity.ShowOnStartPage = model.ShowOnStartPage;
            entity.CreatedDate = model.CreatedDate;
            entity.ChangedDate = model.ChangedDate;
        }
    }
}