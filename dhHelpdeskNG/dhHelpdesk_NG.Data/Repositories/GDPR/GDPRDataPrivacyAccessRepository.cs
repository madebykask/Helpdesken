using System.Linq;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.Repositories.GDPR
{
    public interface IGDPRDataPrivacyAccessRepository : IRepository<GDPRDataPrivacyAccess>
    {
        GDPRDataPrivacyAccess GetUserWithPrivacyPermissionsByUserId(int userId);
    }

    public class GDPRDataPrivacyAccessRepository : RepositoryBase<GDPRDataPrivacyAccess>, IGDPRDataPrivacyAccessRepository
    {
        #region ctor()

        public GDPRDataPrivacyAccessRepository(IDatabaseFactory databaseFactory) 
            : base(databaseFactory)
        {
        }

        #endregion

        public GDPRDataPrivacyAccess GetUserWithPrivacyPermissionsByUserId(int userId)
        {
            var items = this.DataContext.GDPRDataPrivacyAccess.FirstOrDefault(x => x.User_Id == userId && (x.DeletionPermission == 1 || x.AnonymizationPermission == 1));
            return items;
        }
    }
}