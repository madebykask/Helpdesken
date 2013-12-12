namespace dhHelpdesk_NG.Data.Repositories.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;

    public sealed class OrganizationUnitRepository : RepositoryBase<OU>, IOrganizationUnitRepository
    {
        #region Constructors and Destructors

        public OrganizationUnitRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        #endregion

        #region Public Methods and Operators

        public List<ItemOverviewDto> FindActiveAndShowable()
        {
            var organizationUnitOverviews =
                this.DataContext.OUs.Where(u => u.IsActive != 0 && u.Show != 0)
                    .Select(u => new { Id = u.Id, Name = u.Name })
                    .ToList();

            return
                organizationUnitOverviews.Select(
                    u => new ItemOverviewDto { Name = u.Name, Value = u.Id.ToString(CultureInfo.InvariantCulture) })
                                         .ToList();
        }

        #endregion
    }
}