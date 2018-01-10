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
                                            l.Document_Id,
                                            l.CaseSolution_Id,
                                            l.CaseFilterFavorite_Id,
                                            l.CaseFilterFavorite,
                                            l.CaseSolution,
                                            l.OpenInNewWindow,
                                            l.NewWindowWidth,
                                            l.NewWindowHeight
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
                                            DocumentName = l.Document != null ? l.Document.Name : null,
                                            CaseSolutionId = l.CaseSolution_Id,
                                            CaseFilterFavoriteId = l.CaseFilterFavorite_Id,
                                            CaseFilterFavoriteName = l.CaseFilterFavorite != null ? l.CaseFilterFavorite.Name : null,
                                            CaseSolutionName = l.CaseSolution != null ? l.CaseSolution.Name : null,
                                            OpenInNewWindow = l.OpenInNewWindow == 1 ? true : false,
                                            NewWindowHeight = l.NewWindowHeight,
                                            NewWindowWidth = l.NewWindowWidth
                                        }).ToArray();
        }

        public static void MapToEntity(Link model, Link entity)
        {
            entity.Customer_Id = model.Customer_Id;
            entity.CaseSolution_Id = model.CaseSolution_Id;
            entity.CaseFilterFavorite_Id = model.CaseFilterFavorite_Id;
            entity.Document_Id = model.Document_Id;
            entity.LinkGroup_Id = model.LinkGroup_Id;
            entity.NewWindowHeight = model.NewWindowHeight;
            entity.NewWindowWidth = model.NewWindowWidth;
            entity.OpenInNewWindow = model.OpenInNewWindow;
            entity.SortOrder = model.SortOrder;
            entity.URLAddress = model.URLAddress;
            entity.URLName = model.URLName;
            entity.ShowOnStartPage = model.ShowOnStartPage;
            entity.CreatedDate = model.CreatedDate;
            entity.ChangedDate = model.ChangedDate;
        }
    }
}