using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using System.Collections.Generic;
using System.Linq;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region OULANGUAGE

    public interface IOULanguageRepository : IRepository<OULanguage>
    {
    }

    public class OULanguageRepository : RepositoryBase<OULanguage>, IOULanguageRepository
    {
        public OULanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
