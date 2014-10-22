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
                                            d.Size
                                        }).ToArray();

            return entities.Select(d => new DocumentOverview
                                        {
                                            Id = d.Id,
                                            Name = d.Name,
                                            CreatedDate = d.CreatedDate,
                                            Size = d.Size
                                        }).ToArray();
        }
    }
}