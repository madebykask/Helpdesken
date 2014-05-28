// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BulletinBoardRepository.cs" company="">
//   
// </copyright>
// <summary>
//   The BulletinBoardRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.BulletinBoard.Output;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Context;
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
            var entities = this.GetSecuredEntities(this.Table
                .Where(b => customers.Contains(b.Customer_Id))
                .Select(b => new 
                {
                    b.Customer_Id,
                    b.CreatedDate,
                    b.Text,
                    b.ShowOnStartPage,
                    b.WGs
                })
                .OrderByDescending(p => p.CreatedDate)
                .ToList()
                .Select(b => new BulletinBoard
                {
                    Customer_Id = b.Customer_Id,
                    CreatedDate = b.CreatedDate,
                    Text = b.Text,
                    ShowOnStartPage = b.ShowOnStartPage,
                    WGs = b.WGs
                }));
 
            return entities.Select(b => new BulletinBoardOverview
                {
                    CustomerId = b.Customer_Id,
                    CreatedDate = b.CreatedDate,
                    Text = b.Text,
                    ShowOnStartPage = b.ShowOnStartPage.ToBool()
                });
        }
    }
}
