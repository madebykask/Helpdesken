using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    public interface ICaseTypeRepository : IRepository<CaseType>
    {
        void ResetDefault(int exclude);
    }

    public class CaseTypeRepository : RepositoryBase<CaseType>, ICaseTypeRepository
    {
        public CaseTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void ResetDefault(int exclude)
        {
            foreach (CaseType obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
        }

    }
}
