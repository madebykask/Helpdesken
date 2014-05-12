// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ILinkRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Link.Output;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The LinkRepository interface.
    /// </summary>
    public interface ILinkRepository : IRepository<Link>
    {
        /// <summary>
        /// The get link overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<LinkOverview> GetLinkOverviews(int[] customers);
    }

    /// <summary>
    /// The link repository.
    /// </summary>
    public class LinkRepository : RepositoryBase<Link>, ILinkRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public LinkRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// The get link overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<LinkOverview> GetLinkOverviews(int[] customers)
        {
            var entities = this.GetSecuredEntities(this.Table
                .Where(l => l.Customer_Id.HasValue && customers.Contains(l.Customer_Id.Value))
                .Select(l => new
                {
                    l.Customer_Id,
                    l.Customer,
                    l.URLName,
                    l.URLAddress,
                    l.LinkGroup_Id,
                    l.LinkGroup,
                    l.ShowOnStartPage,
                    l.Us,
                    l.SortOrder
                })
                .OrderBy(l => l.SortOrder)
                .ToList()
                .Select(l => new Link
                {
                    Customer_Id = l.Customer_Id,
                    Customer = l.Customer,
                    URLName = l.URLName,
                    URLAddress = l.URLAddress,
                    LinkGroup_Id = l.LinkGroup_Id,
                    LinkGroup = l.LinkGroup,
                    ShowOnStartPage = l.ShowOnStartPage,
                    Us = l.Us,
                    SortOrder = l.SortOrder
                })); 

            return entities                
                .Select(l => new LinkOverview
                {
                    CustomerId = l.Customer_Id,
                    CustomerName = l.Customer != null ? l.Customer.Name : null,
                    UrlName = l.URLName,
                    UrlAddress = l.URLAddress,
                    LinkGroupId = l.LinkGroup_Id,
                    LinkGroupName = l.LinkGroup != null ? l.LinkGroup.LinkGroupName : null,
                    ShowOnStartPage = l.ShowOnStartPage.ToBool(),
                    SortOrder = l.SortOrder
                })
                .OrderBy(l => l.CustomerName)
                .ThenBy(l => l.SortOrder);
        }
    }
}
