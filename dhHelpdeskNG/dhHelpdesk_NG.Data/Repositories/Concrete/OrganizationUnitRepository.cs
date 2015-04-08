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

        public List<ItemOverview> FindActive(int? departmentId)
        {
            var organizationUnitRoot =
                this.DataContext.OUs.Where(u => u.IsActive != 0 && u.Parent_OU_Id == null && u.Department_Id == departmentId)
                    .Select(u => new { Id = u.Id, Name = u.Name })
                    .ToList();

            var organizationUnitFirstChild =
                this.DataContext.OUs.Where(u => u.IsActive != 0 && u.Parent_OU_Id != null && u.Parent.Parent_OU_Id == null && u.Department_Id == departmentId)
                    .Select(u => new { Id = u.Id, Name = u.Parent.Name + " - " + u.Name })
                    .ToList();

            var organizationUnitOverviews =
                organizationUnitRoot.Union(organizationUnitFirstChild);

            return
                organizationUnitOverviews.Select(
                    u => new ItemOverview(u.Name, u.Id.ToString(CultureInfo.InvariantCulture))).OrderBy(x => x.Name).ToList();
        }

        public List<OU> GetOUs(int? departmentId)
        {
            var organizationUnitRoot =
                this.DataContext.OUs.Where(u => u.IsActive != 0 && 
                                                u.Parent_OU_Id == null && 
                                                u.Department_Id == departmentId).ToList();

            var organizationUnitFirstChild =
                this.DataContext.OUs.Where(u => u.IsActive != 0 && 
                                                u.Parent_OU_Id != null && 
                                                u.Parent.Parent_OU_Id == null && 
                                                u.Department_Id == departmentId).ToList();

            foreach (var subOU in organizationUnitFirstChild)
            {
                subOU.Name = subOU.Parent.Name + " - " + subOU.Name;
            }

            var organizationUnitOverviews =
                organizationUnitRoot.Union(organizationUnitFirstChild);

            return
                organizationUnitOverviews.ToList();
        }

        public IEnumerable<OU> GetActiveAndShowable()
        {
            return this.DataContext.OUs.Where(u => u.IsActive != 0 && u.Show != 0);
        }

        #endregion
    }
}