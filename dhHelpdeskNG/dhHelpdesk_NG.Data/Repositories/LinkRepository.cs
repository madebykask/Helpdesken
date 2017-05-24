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
        List<LinkOverview> GetLinkOverviewsToStartPage(int[] customers, int? count, bool forStartPage, int userid);
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

        public List<LinkOverview> GetLinkOverviewsToStartPage(int[] customers, int? count, bool forStartPage, int userid)
        {

            customers.ToString().Split();
            char[] delimiters = new char[] { ',' };
            string[] words = customers.ToString().Split(delimiters);
            string cont = string.Empty;
            foreach (string word in words)
            {
                if (cont == string.Empty)
                {
                    cont = word;
                }
                else
                {
                    cont = cont + " | " + word;
                }
            }

            var userGroups = this.DataContext.UserWorkingGroups.Select(u => u.WorkingGroup_Id);

            var query = this.DataContext.Links.Where(s => s.Customer_Id.ToString().Contains(cont));

            //query = query.Where(x => x.WorkingGroup == null || userGroups.Contains(userid));


            //query = query.Where(x => !x.Us.Any() || x.Us.Any(u => u.Id == workContext.User.UserId));


            return null;
        }
    }
}
