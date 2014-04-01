using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.BulletinBoard.Output;
using DH.Helpdesk.BusinessData.Models.Common.Output;

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
        public BulletinBoardRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<BulletinBoardOverview> GetBulletinBoardOverviews(int[] customers)
        {
            return DataContext.BulletinBoards
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
