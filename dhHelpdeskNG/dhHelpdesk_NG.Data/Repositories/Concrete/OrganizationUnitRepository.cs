namespace DH.Helpdesk.Dal.Repositories.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public sealed class OrganizationUnitRepository : RepositoryBase<OU>, IOrganizationUnitRepository
    {
        #region Constructors and Destructors

        public OrganizationUnitRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        #endregion

        #region Public Methods and Operators

        public List<ItemOverview> FindActiveAndShowable()
        {
            var organizationUnitOverviews =
                this.DataContext.OUs.Where(u => u.IsActive != 0 && u.Show != 0)
                    .Select(u => new { Id = u.Id, Name = u.Name })
                    .ToList();

            return
                organizationUnitOverviews.Select(
                    u => new ItemOverview(u.Name, u.Id.ToString(CultureInfo.InvariantCulture))).OrderBy(x => x.Name).ToList();
        }

        #endregion
    }
}