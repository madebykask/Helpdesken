using DH.Helpdesk.Domain.Invoice;
using LinqLib.Sort;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Department;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;

    public interface IDepartmentService
    {
        IList<Department> GetDepartmentsByUserPermissions(int userId, int customerId, bool isOnlyActive = true);
        
        IList<Department> GetDepartments(int customerId, ActivationStatus isActive = ActivationStatus.Active);
        
        Department GetDepartment(int id);

        DeleteMessage DeleteDepartment(int id);

        void SaveDepartment(Department department,int[] invoiceOus, out IDictionary<string, string> errors);

        void Commit();

        List<ItemOverview> FindActiveOverviews(int customerId);

        ItemOverview FindActiveOverview(int departmentId);

        List<ItemOverview> GetUserDepartments(int customerId, int? userId, int? regionId, int departmentFilterFormat);

        List<ItemOverview> GetDepartmentUsers(int customerId, int? departmentId, int? workingGroupId);

        IEnumerable<Department> GetActiveDepartmentsBy(int customerId, int? regionId);

        IEnumerable<Department> GetDepartmentsByIds(int[] departmentsIds);

        IList<Department> GetChargedDepartments(int customerId);
        bool CheckIfOUsRequireDebit(int departmentId, int? ouId = null);

        int? GetDepartmentIdByCustomerAndName(int customerId, string name);

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

        public IList<Department> GetDepartments(int customerId, ActivationStatus isActive = ActivationStatus.Active)
        {
            if (isActive == ActivationStatus.All)
            {
                return this._departmentRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.DepartmentName).ToList();
            }
               
            return this._departmentRepository.GetMany(x => x.Customer_Id == customerId && x.IsActive == (int)isActive).OrderBy(x => x.DepartmentName).ToList();
        }

        public IList<Department> GetDepartmentsByUserPermissions(int userId, int customerId, bool isOnlyActive = true)
        {
            var departments = _departmentRepository.GetDepartmentsByUserPermissions(userId, customerId, isOnlyActive);
            var res = departments.Where(d => d.Region_Id == null ||
                                             (d.Region != null && d.Region.IsActive != 0) &&
                                             (!isOnlyActive || d.IsActive != 0))
                                 .OrderBy(d => d.DepartmentName)
                                 .ToList();
            return res;
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

        public IEnumerable<Department> GetDepartmentsByIds(int[] departmentsIds)
        {
            var deptMap = departmentsIds.ToDictionary(it => it);
            return this._departmentRepository.GetAll().Where(it => deptMap.ContainsKey(it.Id));
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
            var ous = _ouRepository.GetMany(o => o.Department_Id.HasValue && o.Department_Id == departmentId);
            if (!ous.Any(o => o.ShowInvoice))
                return true;

            if (!ouId.HasValue)
                return false;

            var res = ous.Any(o => o.Id == ouId.Value && o.ShowInvoice);
            return res;            
        }
    }
}
