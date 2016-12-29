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

        void SaveDepartment(Department department, out IDictionary<string, string> errors);

        void Commit();

        List<ItemOverview> FindActiveOverviews(int customerId);

        ItemOverview FindActiveOverview(int departmentId);

        List<ItemOverview> GetUserDepartments(int customerId, int? userId, int? regionId, int departmentFilterFormat);

        List<ItemOverview> GetDepartmentUsers(int customerId, int? departmentId, int? workingGroupId);

        IEnumerable<Department> GetActiveDepartmentsBy(int customerId, int? regionId);

        IEnumerable<Department> GetDepartmentsByIds(int[] departmentsIds);

		IList<Department> GetChargedDepartments(int customerId);
	}

    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository departmentRepository;

        private readonly IUnitOfWork unitOfWork;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork, 
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.departmentRepository = departmentRepository;
            this.unitOfWork = unitOfWork;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public IList<Department> GetDepartments(int customerId, ActivationStatus isActive = ActivationStatus.Active)
        {
            if (isActive == ActivationStatus.All)
            {
                return this.departmentRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.DepartmentName).ToList();
            }
               
            return this.departmentRepository.GetMany(x => x.Customer_Id == customerId && x.IsActive == (int)isActive).OrderBy(x => x.DepartmentName).ToList();
        }

        public IList<Department> GetDepartmentsByUserPermissions(int userId, int customerId, bool isOnlyActive = true)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var usersRep = uow.GetRepository<User>();
                var customersRep = uow.GetRepository<Customer>();
                var departmentsRep = uow.GetRepository<Department>();
                var userDepartmentsRep = uow.GetRepository<DepartmentUser>();

                var users = usersRep.GetAll().GetById(userId);
                var customers = customersRep.GetAll().GetById(customerId);
                var departments = departmentsRep.GetAll().GetActiveByCustomer(customerId);
                var userDepartments = userDepartmentsRep.GetAll();

                var deps = DepartmentMapper.MapToUserDepartments(
                                        users,
                                        customers,
                                        departments,
                                        userDepartments);
                return deps.Where(d => d.Region_Id == null || isOnlyActive == false || (isOnlyActive && d.Region != null && d.Region.IsActive != 0))
                           .ToList();
            }
        }

        public List<ItemOverview> GetUserDepartments(int customerId, int? userId, int? regionId, int departmentFilterFormat)
        {
            using (var uow = this.unitOfWorkFactory.Create())
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
            using (var uow = this.unitOfWorkFactory.Create())
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
            return this.departmentRepository.Get(x => x.Id == id);
        }

        public DeleteMessage DeleteDepartment(int id)
        {           
            var department = this.departmentRepository.GetById(id);

            if (department != null)
            {
                try
                {
                    this.departmentRepository.Delete(department);
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
            return departmentRepository.GetActiveDepartmentsBy(customerId, regionId);
        }

        public IEnumerable<Department> GetDepartmentsByIds(int[] departmentsIds)
        {
            var deptMap = departmentsIds.ToDictionary(it => it);
            return this.departmentRepository.GetAll().Where(it => deptMap.ContainsKey(it.Id));
        }

        public void SaveDepartment(Department department, out IDictionary<string, string> errors)
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

            if (department.Id == 0)
            {
                this.departmentRepository.Add(department);
            }
            else
            {
                this.departmentRepository.Update(department);
            }

            if (errors.Count == 0)
            {
                this.Commit();
            }
        }

        public void Commit()
        {
            this.unitOfWork.Commit();
        }

        public List<ItemOverview> FindActiveOverviews(int customerId)
        {
            return this.departmentRepository.FindActiveOverviews(customerId);
        }

        public ItemOverview FindActiveOverview(int departmentId)
        {
            return this.departmentRepository.FindActiveOverview(departmentId);
        }

		public IList<Department> GetChargedDepartments(int customerId)
		{
			return this.departmentRepository.GetMany(x => x.Customer_Id == customerId && x.Charge == 1)
				.OrderBy(x => x.DepartmentName).ToList();
		}
	}
}
