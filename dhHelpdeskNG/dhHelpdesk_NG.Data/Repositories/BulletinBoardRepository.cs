using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.BulletinBoard.Output;
using DH.Helpdesk.Dal.Infrastructure.Context;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The BulletinBoardRepository interface.
    /// </summary>
    public interface IBulletinBoardRepository : IRepository<BulletinBoard>
    {
        /// <summary>
        /// The get bulletin board overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<BulletinBoardOverview> GetBulletinBoardOverviews(int[] customers);
    }

    /// <summary>
    /// The bulletin board repository.
    /// </summary>
    public class BulletinBoardRepository : RepositoryBase<BulletinBoard>, IBulletinBoardRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BulletinBoardRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        /// <param name="workContext">
        /// The work context.
        /// </param>
        public BulletinBoardRepository(IDatabaseFactory databaseFactory, IWorkContext workContext)
            : base(databaseFactory, workContext)
        {
        }

        /// <summary>
        /// The get bulletin board overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<BulletinBoardOverview> GetBulletinBoardOverviews(int[] customers)
        {
            return GetSecuredEntities()
                .Where(b => customers.Contains(b.Customer_Id))
                .Select(b => new BulletinBoardOverview()
                {
                    CustomerId = b.Customer_Id,
                    CreatedDate = b.CreatedDate,
                    Text = b.Text,
                    ShowOnStartPage = b.ShowOnStartPage.ToBool()
                })
                .OrderByDescending(p => p.CreatedDate)
                .ToList(); 
        }
    }
}
