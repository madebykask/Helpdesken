namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IDepartmentService
    {
        IList<Department> GetDepartmentsByUserPermissions(int userId, int customerId);
        IList<Department> GetDepartments(int customerId, int isActive = 1);
        Department GetDepartment(int id);
        DeleteMessage DeleteDepartment(int id);

        List<ItemOverview> GetOverviews(int customerId, int? regionId); 
        void SaveDepartment(Department department, out IDictionary<string, string> errors);
        void Commit();
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork)
        {
            this._departmentRepository = departmentRepository;
            this._unitOfWork = unitOfWork;
        }

        public IList<Department> GetDepartments(int customerId, int isActive = 1)
        {
            return this._departmentRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.DepartmentName).ToList();
        }

        public IList<Department> GetDepartmentsByUserPermissions(int userId, int customerId)
        {
            var query =this._departmentRepository.GetDepartmentsByUserPermissions(userId, customerId);
            return query != null ? query.ToList() : null;
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

        public List<ItemOverview> GetOverviews(int customerId, int? regionId)
        {
            return !regionId.HasValue
                       ? this._departmentRepository.FindActiveOverviews(customerId)
                       : this._departmentRepository.FindActiveByCustomerIdAndRegionId(customerId, regionId.Value);
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
    }
}
