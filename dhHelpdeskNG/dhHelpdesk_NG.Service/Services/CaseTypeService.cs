namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
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

        IEnumerable<ItemOverview> GetOverviews(int customerId);

        IEnumerable<ItemOverview> GetOverviews(int customerId, IEnumerable<int> caseTypesIds);
    }

    public class CaseTypeService : ICaseTypeService
    {
        private readonly ICaseTypeRepository caseTypeRepository;

        private readonly IUnitOfWork unitOfWork;
        
        public CaseTypeService(
            ICaseTypeRepository caseTypeRepository,
            IUnitOfWork unitOfWork)            
        {
            this.caseTypeRepository = caseTypeRepository;
            this.unitOfWork = unitOfWork;
        }

        public IList<CaseType> GetCaseTypes(int customerId)
        {
            return this.caseTypeRepository.GetMany(x => x.Customer_Id == customerId && x.Parent_CaseType_Id == null).OrderBy(x => x.Name).ToList();
        }

        public CaseType GetCaseType(int id)
        {
            return this.caseTypeRepository.GetById(id);
        }

        public int GetDefaultId(int customerId)
        {
            var r = this.caseTypeRepository.GetMany(x => x.Customer_Id == customerId && x.IsDefault == 1).FirstOrDefault();
            if (r == null)
            {
                return 0;
            }

            return r.Id;
        }

        public DeleteMessage DeleteCaseType(int id)
        {
            var caseType = this.caseTypeRepository.GetById(id);

            if (caseType != null)
            {
                try
                {
                    this.caseTypeRepository.Delete(caseType);
                   
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
            {
                throw new ArgumentNullException("caseType");
            }

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(caseType.Name))
            {
                errors.Add("CaseType.Name", "Du måste ange en ärendetyp");
            }

            caseType.ChangedDate = DateTime.UtcNow;

            if (caseType.Id == 0)
            {
                this.caseTypeRepository.Add(caseType);
            }
            else
            {
                this.caseTypeRepository.Update(caseType);
            }

            if (caseType.IsDefault == 1)
            {
                this.caseTypeRepository.ResetDefault(caseType.Id);
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

        public IEnumerable<ItemOverview> GetOverviews(int customerId)
        {
            return this.caseTypeRepository.GetOverviews(customerId);
        }

        public IEnumerable<ItemOverview> GetOverviews(int customerId, IEnumerable<int> caseTypesIds)
        {
            return this.caseTypeRepository.GetOverviews(customerId, caseTypesIds);
        }
    }
}
