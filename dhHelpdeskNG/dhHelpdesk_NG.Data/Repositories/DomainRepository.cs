namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;

    public interface IDomainRepository : IRepository<Domain.Domain>
    {
        List<ItemOverview> FindByCustomerId(int customerId);

        string GetDomainPassword(int domain_id);

        int GetDomainId(string domainName, int customerId);

    }

    public class DomainRepository : RepositoryBase<Domain.Domain>, IDomainRepository
    {
        public DomainRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindByCustomerId(int customerId)
        {
            var domainOverviews =
                this.DataContext.Domains.Where(d => d.Customer_Id == customerId)
                    .Select(d => new { d.Id, d.Name })
                    .ToList();

            return
                domainOverviews.Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture)))
                    .OrderBy(x => x.Name).ToList();
        }

        public string GetDomainPassword(int domain_id)
        {
          
            var password = (from d in this.DataContext.Domains
                        where d.Id == domain_id
                        select d.Password).First().ToString();

            return password;
        }

        public int GetDomainId(string domainName , int customerId)
        {
            return this.DataContext.Domains.Where(d => d.Name == domainName & d.Customer_Id == customerId).Select(d=> d.Id).FirstOrDefault();

        }
    }
}
