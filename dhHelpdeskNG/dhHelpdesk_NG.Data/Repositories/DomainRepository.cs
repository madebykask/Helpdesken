using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;

namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Globalization;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;

    public interface IDomainRepository : IRepository<Domain.Domain>
    {
        List<ItemOverviewDto> FindByCustomerId(int customerId);

        string GetDomainPassword(int domain_id);
    }

    public class DomainRepository : RepositoryBase<Domain.Domain>, IDomainRepository
    {
        public DomainRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverviewDto> FindByCustomerId(int customerId)
        {
            var domainOverviews =
                this.DataContext.Domains.Where(d => d.Customer_Id == customerId)
                    .Select(d => new { d.Id, d.Name })
                    .ToList();

            return
                domainOverviews.Select(
                    o => new ItemOverviewDto { Name = o.Name, Value = o.Id.ToString(CultureInfo.InvariantCulture) })
                               .ToList();
        }

        public string GetDomainPassword(int domain_id)
        {
          
            var password = (from d in this.DataContext.Domains
                        where d.Id == domain_id
                        select d.Password).ToString();

            return password;
        }
    }
}
