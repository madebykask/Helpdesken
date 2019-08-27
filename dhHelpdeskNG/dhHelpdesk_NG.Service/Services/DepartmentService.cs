using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Dal.NewInfrastructure;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.BusinessLogic.Mappers.Department;
using DH.Helpdesk.Services.BusinessLogic.Specifications;
using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;

namespace DH.Helpdesk.Services.Services
{
    public interface IDepartmentService
    {
        IList<int> GetDepartmentsIdsByUserPermissions(int userId, int customerId, bool isOnlyActive = true);

        Task<List<int>> GetDepartmentsIdsByUserPermissionsAsync(int userId, int customerId, bool isOnlyActive = true);

        IList<Department> GetDepartmentsByUserPermissions(int userId, int customerId, bool isOnlyActive = true);
        
        IList<Department> GetDepartments(int customerId, ActivationStatus isActive = ActivationStatus.Active);

        List<int> GetDepartmentsIds(int customerId, ActivationStatus activeStatus = ActivationStatus.Active);

        Task<List<int>> GetDepartmentsIdsAsync(int customerId, ActivationStatus activeStatus = ActivationStatus.Active);

        Department GetDepartment(int id);

        DeleteMessage DeleteDepartment(int id);

        void SaveDepartment(Department department,int[] invoiceOus, out IDictionary<string, string> errors);

        void Commit();

        List<ItemOverview> FindActiveOverviews(int customerId);

        ItemOverview FindActiveOverview(int departmentId);

        List<ItemOverview> GetUserDepartments(int customerId, int? userId, int? regionId, int departmentFilterFormat);

        List<ItemOverview> GetDepartmentUsers(int customerId, int? departmentId, int? workingGroupId);

        IEnumerable<Department> GetActiveDepartmentsBy(int customerId, int? regionId);

        List<Department> GetDepartmentsByIdsWithHolidays(int[] departmentsIds, int defaultCalendarId);

        IList<Department> GetChargedDepartments(int customerId);
        bool CheckIfOUsRequireDebit(int departmentId, int? ouId = null);

        int? GetDepartmentIdByCustomerAndName(int customerId, string name);
        IList<Department> GetDepartmentsWithRegion(int customerId, ActivationStatus isActive = ActivationStatus.Active);
        List<Department> GetActiveDepartmentForUserByRegion(int? regionId, int userId, int customerId);
        List<Department> GetDepartmentsForUser(int customerId, int userId, bool onlyActive = true);
        bool IsUserDepartment(int departmentId, int userId, int customerId);

    }

    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private readonly IOrganizationUnitRepository _ouRepository;


        public DepartmentService(
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork, 
            IUnitOfWorkFactory unitOfWorkFactory,
            IOrganizationUnitRepository ouRepository)
        {
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _unitOfWorkFactory = unitOfWorkFactory;
            _ouRepository = ouRepository;
        }

        public List<int> GetDepartmentsIds(int customerId, ActivationStatus activeStatus = ActivationStatus.Active)
        {
            var query = GetDepartmentsQueryable(customerId, activeStatus).Select(dep => dep.Id);
            return query.ToList();
        }

        public Task<List<int>> GetDepartmentsIdsAsync(int customerId, ActivationStatus activeStatus = ActivationStatus.Active)
        {
            var query = GetDepartmentsQueryable(customerId, activeStatus).Select(dep => dep.Id);
            return query.ToListAsync();
        }

        public IList<Department> GetDepartments(int customerId, ActivationStatus isActive = ActivationStatus.Active)
        {
            var query = GetDepartmentsQueryable(customerId, isActive);
            return query.ToList();
        }

        private IQueryable<Department> GetDepartmentsQueryable(int customerId, ActivationStatus isActive)
        {
            var query =
                isActive == ActivationStatus.All
                    ? _departmentRepository.GetMany(x => x.Customer_Id == customerId).AsQueryable()
                    : _departmentRepository.GetMany(x => x.Customer_Id == customerId && x.IsActive == (int)isActive).AsQueryable();

            return query.OrderBy(x => x.DepartmentName);
        }

        public IList<Department> GetDepartmentsWithRegion(int customerId, ActivationStatus isActive = ActivationStatus.Active)
        {
            if (isActive == ActivationStatus.All)
            {
                return _departmentRepository.GetMany(d => d.Customer_Id == customerId && (d.Region_Id == null || (d.Region != null && d.Region.IsActive != 0)))
                    .OrderBy(d => d.DepartmentName)
                    .ToList();
            }

            return _departmentRepository.GetMany(d => d.Customer_Id == customerId && d.IsActive == (int)isActive && (d.Region_Id == null || (d.Region != null && d.Region.IsActive != 0)))
                .OrderBy(x => x.DepartmentName)
                .ToList();
        }
        
        public IList<int> GetDepartmentsIdsByUserPermissions(int userId, int customerId, bool isOnlyActive = true)
        {
            return
                GetDepartmentsIdsByUserPermissionsQuery(userId, customerId, isOnlyActive)
                    .ToList();
        }

        public Task<List<int>> GetDepartmentsIdsByUserPermissionsAsync(int userId, int customerId, bool isOnlyActive = true)
        {
            return
                GetDepartmentsIdsByUserPermissionsQuery(userId, customerId, isOnlyActive)                    .ToListAsync();
        }

        public IList<Department> GetDepartmentsByUserPermissions(int userId, int customerId, bool isOnlyActive = true)
        {
            return 
                _departmentRepository.GetDepartmentsByUserPermissions(userId, customerId, isOnlyActive, true).AsQueryable()
                .OrderBy(d => d.DepartmentName)
                .ToList();
        }

        public List<ItemOverview> GetUserDepartments(int customerId, int? userId, int? regionId, int departmentFilterFormat)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var usersRep = uow.GetRepository<User>();
                var customersRep = uow.GetRepository<Customer>();
                var departmentsRep = uow.GetRepository<Department>();
                var userDepartmentsRep = uow.GetRepository<DepartmentUser>();
                var regionRep = uow.GetRepository<Region>();

                var users = usersRep.GetAll().GetById(userId);
                var customers = customersRep.GetAll().GetById(customerId);
                var departments = departmentsRep.GetAll().GetActiveByCustomer(customerId);
                var userDepartments = userDepartmentsRep.GetAll();
                var regions = regionRep.GetAll().GetById(regionId);

                if (userId.HasValue && regionId.HasValue)
                {
                    return DepartmentMapper.MapToUserDepartments(
                                            regions,
                                            users,
                                            departments,
                                            userDepartments,
                                            customers,
                                            departmentFilterFormat,
                                            regionId.Value);                    
                }
                
                if (userId.HasValue)
                {
                    return DepartmentMapper.MapToUserDepartments(
                                            users,
                                            departments,
                                            userDepartments,
                                            customers,
                                            departmentFilterFormat);     
                }

                if (regionId.HasValue)
                {
                    return DepartmentMapper.MapToUserDepartments(
                                            regions,
                                            departments,
                                            customers,
                                            departmentFilterFormat);
                }

                return DepartmentMapper.MapToUserDepartments(
                                            departments,
                                            customers,
                                            departmentFilterFormat);     
            }
        }

        public List<ItemOverview> GetDepartmentUsers(int customerId, int? departmentId, int? workingGroupId)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var usersRep = uow.GetRepository<User>();
                var customersRep = uow.GetRepository<Customer>();
                var departmentsRep = uow.GetRepository<Department>();
                var userDepartmentsRep = uow.GetRepository<DepartmentUser>();
                var customerUsersRep = uow.GetRepository<CustomerUser>();
                var workingGroupsRep = uow.GetRepository<WorkingGroupEntity>();
                var userWorkingGroupsRep = uow.GetRepository<UserWorkingGroup>();

                var users = usersRep.GetAll().GetActive();
                var customers = customersRep.GetAll().GetById(customerId);
                var departments = departmentsRep.GetAll().GetById(departmentId);
                var userDepartments = userDepartmentsRep.GetAll();
                var customerUsers = customerUsersRep.GetAll();
                var workingGroups = workingGroupsRep.GetAll().GetById(workingGroupId);
                var userWorkingGroups = userWorkingGroupsRep.GetAll();

                return DepartmentMapper.MapToDepartmentUsers(
                                        users,
                                        customers,
                                        customerUsers,    
                                        departments,
                                        userDepartments,
                                        workingGroups,
                                        userWorkingGroups,
                                        departmentId,
                                        workingGroupId);
            }
        }

        public Department GetDepartment(int id)
        {
            return this._departmentRepository.Get(x => x.Id == id);
        }

        public DeleteMessage DeleteDepartment(int id)
        {           
            var department = this._departmentRepository.GetById(id);

            if (department != null)
            {
                try
                {
                    this._departmentRepository.Delete(department);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public IEnumerable<Department> GetActiveDepartmentsBy(int customerId, int? regionId)
        {
            return _departmentRepository.GetActiveDepartmentsBy(customerId, regionId);
        }

        public List<Department> GetDepartmentsByIdsWithHolidays(int[] departmentsIds, int defaultCalendarId)
        {
            return _departmentRepository.GetDepartmentsByIds(departmentsIds,  true)
                .Where(it => it.HolidayHeader != null && it.HolidayHeader.Id != defaultCalendarId).ToList();
        }

        public void SaveDepartment(Department department,int[] invoiceOus, out IDictionary<string, string> errors)
        {
            if (department == null)
            {
                throw new ArgumentNullException("department");
            }

            errors = new Dictionary<string, string>();

            department.HeadOfDepartment = department.HeadOfDepartment ?? string.Empty;
            department.HeadOfDepartmentEMail = department.HeadOfDepartmentEMail ?? string.Empty;
            department.SearchKey = department.SearchKey ?? string.Empty;
            department.Path = department.Path ?? string.Empty;
            department.AccountancyAmount = department.AccountancyAmount;
            department.DepartmentId = department.DepartmentId ?? string.Empty;
            department.Charge = department.Charge;
            department.ChargeMandatory = department.ChargeMandatory;
            department.ShowInvoice = department.ShowInvoice;
            department.IsActive = department.IsActive;
            department.IsEMailDefault = department.IsEMailDefault;
            department.ChangedDate = DateTime.UtcNow;
            department.OverTimeAmount = department.OverTimeAmount;
            department.LanguageId = department.LanguageId;
            if (department.Id == 0)
            {
                department.DepartmentGUID = Guid.NewGuid();
                this._departmentRepository.Add(department);
            }
            else
            {
                this._departmentRepository.Update(department);
            }

            if (errors.Count == 0)
            {
                this.Commit();
            }

            var ous = _ouRepository.GetMany(x => x.Department_Id == department.Id);
            if (ous.Any())
            {
                try
                {
                    foreach (var ou in ous)
                    {
                        ou.ShowInvoice = invoiceOus != null && invoiceOus.Contains(ou.Id);
                        _ouRepository.Update(ou);
                    }
                    Commit();
                }
                catch (Exception ex)
                {
                    errors.Add(new KeyValuePair<string, string>("Invoice OUs", ex.Message));
                }
            }
        }

        public int? GetDepartmentIdByCustomerAndName(int customerId, string name)
        {
            var department = this._departmentRepository.Get(x => x.Customer_Id == customerId && x.DepartmentName.ToLower() == name.ToLower());

            if (department != null)
                return department.Id;

            return null;
        }


        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public List<ItemOverview> FindActiveOverviews(int customerId)
        {
            return this._departmentRepository.FindActiveOverviews(customerId);
        }

        public ItemOverview FindActiveOverview(int departmentId)
        {
            return this._departmentRepository.FindActiveOverview(departmentId);
        }

        public IList<Department> GetChargedDepartments(int customerId)
        {
            return this._departmentRepository.GetMany(x => x.Customer_Id == customerId && x.Charge == 1)
                .OrderBy(x => x.DepartmentName).ToList();
        }
        
        public bool CheckIfOUsRequireDebit(int departmentId, int? ouId = null)
        {
            var ous = _ouRepository.GetMany(o => o.Department_Id.HasValue && o.Department_Id == departmentId).ToList();
            if (!ous.Any(o => o.ShowInvoice))
                return true;

            if (!ouId.HasValue)
                return false;

            var res = ous.Any(o => o.Id == ouId.Value && o.ShowInvoice);
            return res;            
        }

        public List<Department> GetActiveDepartmentForUserByRegion(int? regionId, int userId, int customerId)
        {
            var dep = GetDepartmentsByUserPermissions(userId, customerId).ToList();
            if (!dep.Any())
            {
                dep = GetDepartmentsWithRegion(customerId)
                    .Where(d => !d.Region_Id.HasValue || d.Region.IsActive == 1)
                    .ToList();
            }

            if (regionId.HasValue)
            {
                dep = dep.Where(x => x.Region_Id == regionId).ToList();
            }

            return dep;
        }

        public List<Department> GetDepartmentsForUser(int customerId, int userId, bool onlyActive = true)
        {
            var dep = GetDepartmentsByUserPermissions(userId, customerId).ToList();
            if (!dep.Any())
            {
                dep = GetDepartments(customerId, onlyActive ? ActivationStatus.Active : ActivationStatus.All)
                    .ToList();
            }

            return dep;
        }

        public bool IsUserDepartment(int departmentId, int userId, int customerId)
        {
            return _departmentRepository.GetDepartmentsByUserPermissions(userId, customerId, false, true)
                .Any(d => d.Id == departmentId);
        }

        private IQueryable<int> GetDepartmentsIdsByUserPermissionsQuery(int userId, int customerId, bool isOnlyActive)
        {
            return _departmentRepository.GetDepartmentsByUserPermissions(userId, customerId, isOnlyActive, true)
                .Select(d => d.Id);
        }

    }
}
