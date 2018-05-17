using System.Linq;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.Repositories.GDPR
{
    public interface IGDPRDataPrivacyAccessRepository : IRepository<GDPRDataPrivacyAccess>
    {
        GDPRDataPrivacyAccess GetByUserId(int userId);
    }

    public class GDPRDataPrivacyAccessRepository : RepositoryBase<GDPRDataPrivacyAccess>, IGDPRDataPrivacyAccessRepository
    {
        #region ctor()

        public GDPRDataPrivacyAccessRepository(IDatabaseFactory databaseFactory) 
            : base(databaseFactory)
        {
        }

        #endregion

        public GDPRDataPrivacyAccess GetByUserId(int userId)
        {
            var items = this.DataContext.GDPRDataPrivacyAccess.FirstOrDefault(x => x.User_Id == userId);
            return items;
        }
    }
}