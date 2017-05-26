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
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.BusinessData.Models.Link.Output;
    /// <summary>
    /// The LinkRepository interface.
    /// </summary>
    public interface ILinkRepository : Infrastructure.IRepository<Link>
    {
        IEnumerable<LinkOverview> GetLinkOverviewsToStartPage(int[] customers, int? count, bool forStartPage, int userid, IEnumerable<UserWorkingGroup> wg);
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

        public IEnumerable<LinkOverview> GetLinkOverviewsToStartPage(int[] customers, int? count, bool forStartPage, int userid, IEnumerable<UserWorkingGroup> wg)
        {


           



            //Get all links by user
            var query = this.DataContext.Links.Where(x => !x.Us.Any() || x.Us.Any(u => u.Id == userid));

        
            //Add user groups condition
            var userGroups = wg.Select(u => u.WorkingGroup_Id);
            query = query.Where(x => !x.Wg.Any() || x.Wg.Any(g => userGroups.Contains(g.Id)));

            //Filter by customer
            query = query.Where(l => l.Customer_Id.HasValue && customers.Contains(l.Customer_Id.Value));


           // query = query.Where(x => customers.Contains(x.Customer_Id));


            if (forStartPage)
            {
                query = query.Where(l => l.ShowOnStartPage == 1);
            }

            query = query.OrderBy(l => l.Customer.Name)
                        .ThenBy(l => l.SortOrder);

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }


            List<LinkOverview> llist = new List<LinkOverview>();


            foreach (var x in query)
            {
                LinkOverview ltemp = new LinkOverview();
                ltemp.CaseSolutionId = x.CaseSolution_Id != null ? x.CaseSolution_Id : 0;
                if (x.CaseSolution != null)
                {
                    ltemp.CaseSolutionName = x.CaseSolution.Name != null ? x.CaseSolution.Name : string.Empty;
                }
                else
                {
                    ltemp.CaseSolutionName = string.Empty;
                }
                ltemp.CustomerId = x.Customer_Id != null ? x.Customer_Id : 0;
                if (x.Customer != null)
                {
                    ltemp.CustomerName = x.Customer.Name != null ? x.Customer.Name : string.Empty;
                }
                else
                {
                    ltemp.CustomerName = string.Empty;
                }
                ltemp.DocumentId = x.Document_Id != null ? x.Document_Id : 0;
                if (x.Document != null)
                {
                    ltemp.DocumentName = x.Document.Name != null ? x.Document.Name : string.Empty;
                }
                else
                {
                    ltemp.DocumentName = string.Empty;
                }
                ltemp.LinkGroupId = x.LinkGroup_Id != null ? x.LinkGroup_Id : 0;
                if (x.LinkGroup != null)
                {
                    ltemp.LinkGroupName = x.LinkGroup.LinkGroupName != null ? x.LinkGroup.LinkGroupName : string.Empty;
                }
                else
                {
                    ltemp.LinkGroupName = string.Empty;
                }
                ltemp.NewWindowHeight = x.NewWindowHeight != null ? x.NewWindowHeight : 0;
                ltemp.NewWindowWidth = x.NewWindowWidth != null ? x.NewWindowWidth : 0;
                ltemp.OpenInNewWindow = x.OpenInNewWindow != null ? Convert.ToBoolean(x.OpenInNewWindow) : false;
                ltemp.ShowOnStartPage = x.ShowOnStartPage != null ? Convert.ToBoolean(x.ShowOnStartPage) : false;
                ltemp.SortOrder = x.SortOrder != null ? x.SortOrder : string.Empty;
                ltemp.UrlAddress = x.URLAddress != null ? x.URLAddress : string.Empty;
                ltemp.UrlName = x.URLName != null ? x.URLName : string.Empty;


                llist.Add(ltemp);
            }


            return llist;
        }
    }
}
