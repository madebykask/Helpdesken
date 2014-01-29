using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IOUService
    {
        IList<OU> GetAllOUs();
        IList<OU> GetThemAllOUs(int customerId);
        IList<OU> GetOuForDepartment(int departmentId);
        IList<OU> GetOUs(int customerId);
        OU GetOU(int id);

        DeleteMessage DeleteOU(int id);

        void SaveOU(OU ou, out IDictionary<string, string> errors);
        void Commit();
    }

    public class OUService : IOUService
    {
        private readonly IOrganizationUnitRepository _ouRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OUService(IOrganizationUnitRepository ouRepository,
            IUnitOfWork unitOfWork)
        {
            _ouRepository = ouRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<OU> GetAllOUs()
        {
            return _ouRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        public IList<OU> GetThemAllOUs(int customerId)
        {
            return _ouRepository.GetMany(x => x.Department.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<OU> GetOuForDepartment(int departmentId)
        {
            return _ouRepository.GetMany(x => x.Department_Id == departmentId).OrderBy(x => x.Name).ToList();
        }

        public IList<OU> GetOUs(int customerId)
        {
            return _ouRepository.GetMany(x => x.Parent_OU_Id == null && x.Department.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public OU GetOU(int id)
        {
            return _ouRepository.Get(x => x.Id == id);
        }

        public DeleteMessage DeleteOU(int id)
        {
            var ou = _ouRepository.GetById(id);

            if (ou != null)
            {
                try
                {
                    _ouRepository.Delete(ou);

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

        public void SaveOU(OU ou, out IDictionary<string, string> errors)
        {
            if (ou == null)
                throw new ArgumentNullException("ou");

           
            ou.Path = ou.Path ?? string.Empty;
            ou.OUId = ou.OUId ?? string.Empty;
            ou.HomeDirectory = ou.HomeDirectory ?? string.Empty;
            ou.ScriptPath = ou.ScriptPath ?? string.Empty;
                       
            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(ou.Name))
                errors.Add("ou.Name", "Du måste ange en organisationsenhet");


            if (ou.Id == 0)
                _ouRepository.Add(ou);
            else
                _ouRepository.Update(ou);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
