namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Department;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;
    using DH.Helpdesk.Common.Enums;

    public interface IDepartmentService
    {
        IList<Department> GetDepartmentsByUserPermissions(int userId, int customerId);
        IList<Department> GetDepartments(int customerId, ActivationStatus isActive = ActivationStatus.Active);
        Department GetDepartment(int id);
        DeleteMessage DeleteDepartment(int id);

        void SaveDepartment(Department department, out IDictionary<string, string> errors);
        void Commit();

        List<ItemOverview> FindActiveOverviews(int customerId);

        ItemOverview FindActiveOverview(int departmentId);

        List<ItemOverview> GetUserDepartments(int customerId, int? userId, int? regionId, int departmentFilterFormat);

        List<ItemOverview> GetDepartmentUsers(int customerId, int? departmentId);
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork, 
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            this._departmentRepository = departmentRepository;
            this._unitOfWork = unitOfWork;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public IList<Department> GetDepartments(int customerId, ActivationStatus isActive = ActivationStatus.Active)
        {            
            if (isActive == ActivationStatus.All)
               return this._departmentRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.DepartmentName).ToList();
            else
               return this._departmentRepository.GetMany(x => x.Customer_Id == customerId && x.IsActive == (int)isActive).OrderBy(x => x.DepartmentName).ToList();
        }

        public IList<Department> GetDepartmentsByUserPermissions(int userId, int customerId)
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

                return DepartmentMapper.MapToUserDepartments(
                                        users,
                                        customers,
                                        departments,
                                        userDepartments);
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

        public List<ItemOverview> GetDepartmentUsers(int customerId, int? departmentId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var usersRep = uow.GetRepository<User>();
                var customersRep = uow.GetRepository<Customer>();
                var departmentsRep = uow.GetRepository<Department>();
                var userDepartmentsRep = uow.GetRepository<DepartmentUser>();
                var customerUsersRep = uow.GetRepository<CustomerUser>();

                var users = usersRep.GetAll();
                var customers = customersRep.GetAll().GetById(customerId);
                var departments = departmentsRep.GetAll().GetById(departmentId);
                var userDepartments = userDepartmentsRep.GetAll();
                var customerUsers = customerUsersRep.GetAll();

                if (departmentId.HasValue)
                {
                    return DepartmentMapper.MapToDepartmentUsers(
                                            users,
                                            customers,
                                            departments,
                                            userDepartments);
                }

                return UsersMapper.MapToCustomerUsersOverviews(customers, users, customerUsers);
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

        public void SaveDepartment(Department department, out IDictionary<string, string> errors)
        {
            if (department == null)
                throw new ArgumentNullException("department");

            errors = new Dictionary<string, string>();

            department.HeadOfDepartment = department.HeadOfDepartment ?? "";
            department.HeadOfDepartmentEMail = department.HeadOfDepartmentEMail ?? "";
            department.SearchKey = department.SearchKey ?? "";
            department.Path = department.Path ?? "";
            department.AccountancyAmount = department.AccountancyAmount;
            department.DepartmentId = department.DepartmentId ?? "";
            department.Charge = department.Charge;
            department.ChargeMandatory = department.ChargeMandatory;
            department.ShowInvoice = department.ShowInvoice;
            department.IsActive = department.IsActive;
            department.IsEMailDefault = department.IsEMailDefault;
            department.ChangedDate = DateTime.UtcNow;
            department.OverTimeAmount = department.OverTimeAmount;
            
            if (department.Id == 0)
                this._departmentRepository.Add(department);
            else
                this._departmentRepository.Update(department);

            if (errors.Count == 0)
                this.Commit();
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
    }
}
