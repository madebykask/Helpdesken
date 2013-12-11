using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;

namespace dhHelpdesk_NG.Data.Repositories
{
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;

    public interface IDomainRepository : IRepository<Domain.Domain>
    {
        List<DomainOverviewDto> FindByCustomerId(int customerId);

        string GetDomainPassword(int domain_id);
    }

    public class DomainRepository : RepositoryBase<Domain.Domain>, IDomainRepository
    {
        public DomainRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<DomainOverviewDto> FindByCustomerId(int customerId)
        {
            return
                this.DataContext.Domains.Where(d => d.Customer_Id == customerId)
                    .Select(d => new DomainOverviewDto { Id = d.Id, Name = d.Name })
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
