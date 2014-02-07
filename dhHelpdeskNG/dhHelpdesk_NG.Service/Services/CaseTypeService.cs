namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface ICaseTypeService
    {
        IList<CaseType> GetCaseTypes(int customerId);
        CaseType GetCaseType(int id);
        DeleteMessage DeleteCaseType(int id);
        int GetDefaultId(int customerId); 

        void SaveCaseType(CaseType caseType, out IDictionary<string, string> errors);
        void Commit();
    }

    public class CaseTypeService : ICaseTypeService
    {
        private readonly ICaseTypeRepository _caseTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        
        public CaseTypeService(
            ICaseTypeRepository caseTypeRepository,
            IUnitOfWork unitOfWork)            
        {
            this._caseTypeRepository = caseTypeRepository;
            this._unitOfWork = unitOfWork;
        }

        public IList<CaseType> GetCaseTypes(int customerId)
        {
            return this._caseTypeRepository.GetMany(x => x.Customer_Id == customerId && x.Parent_CaseType_Id == null).OrderBy(x => x.Name).ToList();
        }

        public CaseType GetCaseType(int id)
        {
            return this._caseTypeRepository.GetById(id);
        }


        public int GetDefaultId(int customerId)
        {
            var r = this._caseTypeRepository.GetMany(x => x.Customer_Id == customerId && x.IsDefault == 1).FirstOrDefault();
            if (r == null)
                return 0;
            return r.Id;
        }

        public DeleteMessage DeleteCaseType(int id)
        {
            var caseType = this._caseTypeRepository.GetById(id);

            if (caseType != null)
            {
                try
                {
                    this._caseTypeRepository.Delete(caseType);
                   
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

        public void SaveCaseType(CaseType caseType, out IDictionary<string, string> errors)
        {
            if (caseType == null)
                throw new ArgumentNullException("casetype");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(caseType.Name))
                errors.Add("CaseType.Name", "Du måste ange en ärendetyp");

            caseType.ChangedDate = DateTime.UtcNow;

            if (caseType.Id == 0)
                this._caseTypeRepository.Add(caseType);
            else
                this._caseTypeRepository.Update(caseType);

            if (caseType.IsDefault == 1)
                this._caseTypeRepository.ResetDefault(caseType.Id);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
