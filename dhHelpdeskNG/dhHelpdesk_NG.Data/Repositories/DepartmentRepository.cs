using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.NewInfrastructure;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region DEPARTMENT

    public interface IDepartmentRepository : IRepository<Department>
    {
        IEnumerable<Department> GetDepartmentsForUser(int userId, int customerId = 0);
        IEnumerable<Department> GetDepartmentsByUserPermissions(int userId, int customerId, bool activeOnly = true);
        void ResetDefault(int exclude);

        List<ItemOverview> FindActiveOverviews(int customerId);
        List<ItemOverview> FindActiveByCustomerIdAndRegionId(int customerId, int regionId);

        string GetDepartmentName(int departmentId);

        ItemOverview FindActiveOverview(int departmentId);

        IEnumerable<Department> GetActiveDepartmentsBy(int customerId, int? regionId = null);

        int GetDepartmentLanguage(int departmentId);

        void UpdateDeparmentDisabledForOrder(int[] toEnables, int[] toDisable);

        int GetDepartmentId(string departmentName, int customerId);
    }

    public sealed class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<Department> GetDepartmentsForUser(int userId, int customerId = 0)
        {
            var query = from d in this.DataContext.Departments
                        join cu in this.DataContext.CustomerUsers on d.Customer_Id equals cu.Customer_Id
                        join u in this.DataContext.Users on cu.User_Id equals u.Id
                        where cu.User_Id == userId && (cu.Customer_Id == customerId || customerId == 0)
                            && d.IsActive != 0
                        select d;

            return query.OrderBy(x => x.DepartmentName);
        }

        public IEnumerable<Department> GetDepartmentsByUserPermissions(int userId, int customerId, bool activeOnly = true)
        {
            var query = from d in Table
                        from du in d.Users
                        where d.Customer_Id == customerId && du.Id == userId
                        select d;

            query.IncludePath(d => d.Country);

            return activeOnly
                ? query.Where(d => d.IsActive > 0).ToList()
                : query.ToList();
        }
        
        public void ResetDefault(int exclude)
        {
            foreach (var obj in this.GetMany(s => s.IsEMailDefault == 1 && s.Id != exclude))
            {
                obj.IsEMailDefault = 0;
                this.Update(obj);
            }
        }

        public List<ItemOverview> FindActiveOverviews(int customerId)
        {
            var departmentOverviews =
                this.DataContext.Departments.Where(d => d.Customer_Id == customerId && d.IsActive != 0 && 
                                                       (d.Region_Id == null || (d.Region != null && d.Region.IsActive != 0)))
                    .Select(d => new { d.Id, d.DepartmentName })
                    .ToList();

            return
                departmentOverviews.Select(
                    o => new ItemOverview(o.DepartmentName, o.Id.ToString(CultureInfo.InvariantCulture))).OrderBy(x => x.Name).ToList();
        }

        public List<ItemOverview> FindActiveByCustomerIdAndRegionId(int customerId, int regionId)
        {
            var departmentOverviews =
                this.DataContext.Departments.Where(
                    d => d.Customer_Id == customerId && d.Region_Id == regionId && d.IsActive != 0)
                    .Select(d => new { d.Id, d.DepartmentName })
                    .ToList();

            return
                departmentOverviews.Select(
                    o => new ItemOverview(o.DepartmentName, o.Id.ToString(CultureInfo.InvariantCulture))).OrderBy(x => x.Name).ToList();
        }

        public int GetDepartmentLanguage(int departmentId)
        {
            return this.DataContext.Departments.Where(d => d.Id == departmentId).Select(d => d.LanguageId).FirstOrDefault();
        }

        public string GetDepartmentName(int departmentId)
        {
            return this.DataContext.Departments.Where(d => d.Id == departmentId).Select(d => d.DepartmentName).FirstOrDefault();
        }

        public ItemOverview FindActiveOverview(int departmentId)
        {
            var departments =
                this.DataContext.Departments
                    .Where(d =>
                            d.Id == departmentId &&
                            d.IsActive != 0 && (d.Region_Id == null || (d.Region != null && d.Region.IsActive != 0)))
                    .Select(d => new { d.Id, d.DepartmentName })
                    .ToList();

            return
                departments.Select(
                    d => new ItemOverview(d.DepartmentName, d.Id.ToString(CultureInfo.InvariantCulture)))
                    .FirstOrDefault();            
        }

        /// <summary>
        /// Returns active depratments for customer by regionId
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public IEnumerable<Department> GetActiveDepartmentsBy(int customerId, int? regionId = null)
        {
            var query = this.DataContext.Departments.Where(d => d.Customer_Id == customerId && d.IsActive != 0 && 
                                                               (d.Region_Id == null || (d.Region != null && d.Region.IsActive != 0)));                
            if (regionId.HasValue)
            {
                return query.Where(it => it.Region_Id == regionId.Value).OrderBy(d => d.DepartmentName);
            }

            return query.OrderBy(d => d.DepartmentName);
        }

        public void UpdateDeparmentDisabledForOrder(int[] toEnables, int[] toDisable)
        {
            /*Enabling*/
            foreach (var obj in GetMany(d => toEnables.Contains(d.Id)))
            {
                obj.DisabledForOrder = false;
                Update(obj);
            }

            /*Disabling*/
            foreach (var obj in GetMany(d => toDisable.Contains(d.Id)))
            {
                obj.DisabledForOrder = true;
                Update(obj);
            }
        }

        public int GetDepartmentId(string departmentName, int customerId)
        {           
            return this.DataContext.Departments.Where(d => d.DepartmentName == departmentName & d.Customer_Id == customerId).Select(d => d.Id).FirstOrDefault();           
        }
    }

    #endregion

    #region DEPARTMENTUSER

    public interface IDepartmentUserRepository : IRepository<DepartmentUser>
    {
        
    }

    public class DepartmentUserRepository : RepositoryBase<DepartmentUser>, IDepartmentUserRepository
    {
        public DepartmentUserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
       
    }

    #endregion
}
