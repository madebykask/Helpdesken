using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.Repositories.GDPR
{
    public interface IGDPRDataPrivacyFavoriteRepository : IRepository<GDPRDataPrivacyFavorite>
    {
        IDictionary<int, string> ListFavorites();
    }

    public class GDPRDataPrivacyFavoriteRepository : RepositoryBase<GDPRDataPrivacyFavorite>, IGDPRDataPrivacyFavoriteRepository
    {
        #region ctor()

        public GDPRDataPrivacyFavoriteRepository(IDatabaseFactory databaseFactory, IWorkContext workContext = null) 
            : base(databaseFactory, workContext)
        {
        }

        #endregion

        public IDictionary<int, string> ListFavorites()
        {
            return this.Table.ToDictionary(x => x.Id, x => x.Name);
        }
    }
}