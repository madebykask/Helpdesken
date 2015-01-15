namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Links
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Link.Output;
    using DH.Helpdesk.Domain;

    public static class LinkMapper
    {
        public static LinkOverview[] MapToOverviews(this IQueryable<Link> query)
        {
            var entities = query.Select(l => new
                                        {
                                            l.Customer_Id,
                                            l.Customer,
                                            l.LinkGroup_Id,
                                            l.LinkGroup,
                                            l.URLAddress,
                                            l.URLName,
                                            l.SortOrder,
                                            l.Document,
                                            l.Document_Id
                                        }).ToArray();

            return entities.Select(l => new LinkOverview
                                        {
                                            CustomerId = l.Customer_Id,
                                            CustomerName = l.Customer != null ? l.Customer.Name : null,
                                            LinkGroupId = l.LinkGroup_Id,
                                            LinkGroupName = l.LinkGroup != null ? l.LinkGroup.LinkGroupName : null,
                                            UrlAddress = l.URLAddress,
                                            UrlName = l.URLName,
                                            SortOrder = l.SortOrder,
                                            DocumentId = l.Document_Id,
                                            DocumentName = l.Document != null ? l.Document.Name : null
                                        }).ToArray();
        }
    }
}