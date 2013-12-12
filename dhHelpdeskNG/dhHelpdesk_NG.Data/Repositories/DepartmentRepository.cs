using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Globalization;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;

    #region DEPARTMENT

    public interface IDepartmentRepository : IRepository<Department>
    {
        IEnumerable<Department> GetDepartmentsForUser(int userId, int customerId = 0);
        IEnumerable<Department> GetDepartmentsByUserPermissions(int userId, int customerId);
        void ResetDefault(int exclude);

        List<ItemOverviewDto> FindActiveByCustomerId(int customerId);
        List<ItemOverviewDto> FindActiveByCustomerIdAndRegionId(int customerId, int regionId);
    }

    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<Department> GetDepartmentsForUser(int userId, int customerId = 0)
        {
            var query = from d in DataContext.Departments
                        join cu in DataContext.CustomerUsers on d.Customer_Id equals cu.Customer_Id
                        join u in DataContext.Users on cu.User_Id equals u.Id
                        where cu.User_Id == userId && (cu.Customer_Id == customerId || customerId == 0)
                        select d;

            return query.OrderBy(x => x.DepartmentName);
        }

        public IEnumerable<Department> GetDepartmentsByUserPermissions(int userId, int customerId)
        {
            var query = from d in DataContext.Departments
                        join du in DataContext.DepartmentUsers on d.Id equals du.Department_Id 
                        where d.Customer_Id == customerId && du.User_Id == userId 
                        select d;
            return query.Any() ? query.OrderBy(x => x.DepartmentName) : null;
        }
        
        public void ResetDefault(int exclude)
        {
            foreach (var obj in GetMany(s => s.IsEMailDefault == 1 && s.Id != exclude))
            {
                obj.IsEMailDefault = 0;
                Update(obj);
            }
        }

        public List<ItemOverviewDto> FindActiveByCustomerId(int customerId)
        {
            var departmentOverviews =
                this.DataContext.Departments.Where(d => d.Customer_Id == customerId && d.IsActive != 0)
                    .Select(d => new { d.Id, d.DepartmentName })
                    .ToList();

            return
                departmentOverviews.Select(
                    o =>
                    new ItemOverviewDto { Name = o.DepartmentName, Value = o.Id.ToString(CultureInfo.InvariantCulture) })
                                   .ToList();
        }

        public List<ItemOverviewDto> FindActiveByCustomerIdAndRegionId(int customerId, int regionId)
        {
            var departmentOverviews =
                this.DataContext.Departments.Where(
                    d => d.Customer_Id == customerId && d.Region_Id == regionId && d.IsActive != 0)
                    .Select(d => new { d.Id, d.DepartmentName })
                    .ToList();

            return
                departmentOverviews.Select(
                    o =>
                    new ItemOverviewDto { Name = o.DepartmentName, Value = o.Id.ToString(CultureInfo.InvariantCulture) })
                                   .ToList();
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
