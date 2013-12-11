namespace dhHelpdesk_NG.Data.Repositories.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public sealed class OrganizationUnitRepository : RepositoryBase<OU>, IOrganizationUnitRepository
    {
        public OrganizationUnitRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<OrganizationUnitOverviewDto> FindActiveAndShowable()
        {
            return
                this.DataContext.OUs.Where(u => u.IsActive != 0 && u.Show != 0)
                    .Select(u => new OrganizationUnitOverviewDto { Id = u.Id, Name = u.Name })
                    .ToList();
        }
    }
}
