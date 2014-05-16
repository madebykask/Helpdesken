namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ICaseTypeRepository : IRepository<CaseType>
    {
        void ResetDefault(int exclude);

        IEnumerable<ItemOverview> GetOverviews(int customerId);
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

        public IEnumerable<ItemOverview> GetOverviews(int customerId)
        {
            var entities = this.Table
                    .Where(g => g.Customer_Id == customerId && g.IsActive == 1)
                    .Select(g => new { Value = g.Id, g.Name })
                    .OrderBy(g => g.Name)
                    .ToList();

            return entities.Select(g => new ItemOverview(g.Name, g.Value.ToString(CultureInfo.InvariantCulture)));            
        }
    }
}
