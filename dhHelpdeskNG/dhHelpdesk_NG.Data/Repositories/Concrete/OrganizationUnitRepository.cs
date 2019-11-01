using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DH.Helpdesk.Common.Extensions.Boolean;

namespace DH.Helpdesk.Dal.Repositories.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Data.Entity;

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

        public List<ItemOverview> FindActiveByCustomer(int customerId)
        {
            var organizationUnitRoot =
                this.DataContext.OUs.Where(u => u.IsActive != 0 && u.Parent_OU_Id == null && u.Department.Customer_Id == customerId)
                    .Select(u => new { Id = u.Id, Name = u.Name })
                    .ToList();

            var organizationUnitFirstChild =
                this.DataContext.OUs.Where(u => u.IsActive != 0 && u.Parent_OU_Id != null && u.Parent.Parent_OU_Id == null && u.Department.Customer_Id == customerId)
                    .Select(u => new { Id = u.Id, Name = u.Parent.Name + " - " + u.Name })
                    .ToList();

            var organizationUnitOverviews =
                organizationUnitRoot.Union(organizationUnitFirstChild);

            return
                organizationUnitOverviews.Select(
                    u => new ItemOverview(u.Name, u.Id.ToString(CultureInfo.InvariantCulture))).OrderBy(x => x.Name).ToList();
        }

        public IEnumerable<OU> GetRootOUs(int customerId, bool includeSubOu = false)
        {
            var query = DataContext.OUs.AsNoTracking()
                .Where(x => x.Parent_OU_Id == null && x.Department.Customer_Id == customerId);
            if (includeSubOu)
                query = query.Include(u => u.SubOUs);
            return query.OrderBy(x => x.Name);
        }

        public List<OU> GetOUs(int? departmentId)
        {
            var organizationUnitAll =
                this.DataContext.OUs.AsNoTracking()
                    .Include(u => u.Parent).Where(u => u.IsActive != 0 && 
                                                (u.Parent_OU_Id == null ||
                                                 (u.Parent_OU_Id != null && 
                                                 u.Parent.Parent_OU_Id == null)) && 
                                                u.Department_Id == departmentId).ToList();

            var organizationUnitFirstChild = organizationUnitAll.Where(u => u.Parent_OU_Id != null && 
                                                                             u.Parent.Parent_OU_Id == null)
                                                                .ToList();

            foreach (var subOU in organizationUnitFirstChild)
            {
                subOU.Name = subOU.Parent.Name + " - " + subOU.Name;
            }

            return organizationUnitAll;
        }

        public List<OU> GetCustomerOUs(int customerId)
        {
            var organizationUnitRoot =
                this.DataContext.OUs.Where(u => u.IsActive != 0 &&
                                                u.Department != null && 
                                                u.Department.Customer_Id == customerId &&
                                                u.Parent_OU_Id == null).ToList();

            var organizationUnitFirstChild =
                this.DataContext.OUs.Where(u => u.IsActive != 0 &&
                                                u.Department != null &&
                                                u.Department.Customer_Id == customerId &&
                                                u.Parent_OU_Id != null &&
                                                u.Parent.Parent_OU_Id == null).ToList();

            foreach (var subOU in organizationUnitFirstChild)
            {
                subOU.Name = subOU.Parent.Name + " - " + subOU.Name;
            }

            var organizationUnitOverviews =
                organizationUnitRoot.Union(organizationUnitFirstChild);

            return
                organizationUnitOverviews.ToList();
        }

        public List<OU> GetOUs(int customerId, int departmentId, bool? isActive = null)
        {
            return GetOUsQuery(customerId, departmentId, isActive).ToList();
        }

        public async Task<List<OU>> GetOUsAsync(int customerId, int departmentId, bool? isActive = null)
        {
            return await GetOUsQuery(customerId, departmentId, isActive).ToListAsync();
        }

        public IEnumerable<OU> GetActiveAndShowable()
        {
            return this.DataContext.OUs.Where(u => u.IsActive != 0 && u.Show != 0);
        }

        private IQueryable<OU> GetOUsQuery(int customerId, int departmentId, bool? isActive = null)
        {
            var query = DataContext.OUs.Include(x => x.SubOUs)
                .Where(x => x.Parent_OU_Id == null &&
                            x.Department.Customer_Id == customerId &&
                            x.Department_Id == departmentId);
            if (isActive.HasValue)
            {
                var isActiveInt = isActive.Value.ToInt();
                query = query.Where(x => x.IsActive == isActiveInt);
            }

            return query;
        }

        #endregion
    }
}