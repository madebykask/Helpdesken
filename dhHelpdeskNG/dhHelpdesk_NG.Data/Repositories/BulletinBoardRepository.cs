using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.BulletinBoard.Output;
using DH.Helpdesk.BusinessData.Models.Common.Output;
using DH.Helpdesk.Dal.Infrastructure.Context;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IBulletinBoardRepository : IRepository<BulletinBoard>
    {
        IEnumerable<BulletinBoardOverview> GetBulletinBoardOverviews(int[] customers);
    }

    public class BulletinBoardRepository : RepositoryBase<BulletinBoard>, IBulletinBoardRepository
    {
        public BulletinBoardRepository(IDatabaseFactory databaseFactory,
            IWorkContext workContext)
            : base(databaseFactory, workContext)
        {
        }

        public IEnumerable<BulletinBoardOverview> GetBulletinBoardOverviews(int[] customers)
        {
            return GetSecuredEntities()
                .Where(b => customers.Contains(b.Customer_Id))
                .Select(b => new BulletinBoardOverview()
                {
                    Customer_Id = b.Customer_Id,
                    CreatedDate = b.CreatedDate,
                    Text = b.Text
                })
                .OrderByDescending(p => p.CreatedDate); 
        }
    }
}
