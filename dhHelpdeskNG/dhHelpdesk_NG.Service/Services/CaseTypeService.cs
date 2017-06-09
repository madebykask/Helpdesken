namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Remoting.Messaging;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface ICaseTypeService
    {
        IList<CaseType> GetCaseTypes(int customerId, bool isTakeOnlyActive = false);

        IList<CaseType> GetAllCaseTypes(int customerId, bool isTakeOnlyActive = false);

        CaseType GetCaseType(int id);

        DeleteMessage DeleteCaseType(int id);

        int GetDefaultId(int customerId); 

        void SaveCaseType(CaseType caseType, out IDictionary<string, string> errors);

        void Commit();

        IEnumerable<ItemOverview> GetOverviews(int customerId);

        IEnumerable<ItemOverview> GetOverviews(int customerId, IEnumerable<int> caseTypesIds);

        IList<CaseType> GetChildrenInRow(IList<CaseType> caseTypes, bool isTakeOnlyActive = false);

        string GetCaseTypeFullName(int caseTypeId);
    }

    public class CaseTypeService : ICaseTypeService
    {
        private readonly ICaseTypeRepository caseTypeRepository;

        private readonly IUnitOfWork unitOfWork;

        private readonly ICaseRepository _caseRepository;
        
        public CaseTypeService(
            ICaseTypeRepository caseTypeRepository,
            ICaseRepository caseRepository,
            IUnitOfWork unitOfWork)            
        {
            this.caseTypeRepository = caseTypeRepository;
            this.unitOfWork = unitOfWork;
            this._caseRepository = caseRepository;
        }

        public IList<CaseType> GetCaseTypes(int customerId, bool isTakeOnlyActive = false)
        {
            var query = this.caseTypeRepository.GetMany(
                x => x.Customer_Id == customerId && x.Parent_CaseType_Id == null);
            if (isTakeOnlyActive)
            {
                query = query.Where(it => it.IsActive == 1 && it.Selectable == 1);
            }

            return query.OrderBy(x => x.Name).ToList();
        }

        public IList<CaseType> GetAllCaseTypes(int customerId, bool isTakeOnlyActive = false)
        {
            var query = this.caseTypeRepository.GetMany(
                x => x.Customer_Id == customerId);
            if (isTakeOnlyActive)
            {
                query = query.Where(it => it.IsActive == 1 && it.Selectable == 1);
            }

            return query.OrderBy(x => x.Name).ToList();
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
            
            if (caseType != null && !this._caseRepository.GetCasesIdsByType(id).Any())
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
                caseType.CaseTypeGUID = Guid.NewGuid();
                this.caseTypeRepository.Add(caseType);
            }
            else
            {
                this.caseTypeRepository.Update(caseType);
            }

            if (caseType.IsDefault == 1)
            {
                this.caseTypeRepository.ResetDefault(caseType.Id, caseType.Customer_Id);
            }

            if (caseType.IsEMailDefault == 1)
            {
                this.caseTypeRepository.ResetEmailDefault(caseType.Id, caseType.Customer_Id);
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

        public IList<CaseType> GetChildrenInRow(IList<CaseType> caseTypes, bool isTakeOnlyActive = false)
        {
            var childCaseTypes = new List<CaseType>();
            var parentCaseTypes = caseTypes.Where(ct=> !ct.Parent_CaseType_Id.HasValue && (isTakeOnlyActive? ct.IsActive == 1: true)).ToList();
            foreach (var p in parentCaseTypes)
            {
                childCaseTypes.AddRange(GetChilds(p.Name, p.IsActive, p.SubCaseTypes.ToList(), isTakeOnlyActive));
            }

            return parentCaseTypes.Union(childCaseTypes).OrderBy(c => c.Name).ToList();
        }

        public string GetCaseTypeFullName(int caseTypeId)
        {
            var allCaseTypes = this.caseTypeRepository.GetAll().ToList();            
            return GetCaseTypeFullNameById(caseTypeId, allCaseTypes);
        }

        private string GetCaseTypeFullNameById(int caseTypeId, List<CaseType> caseTypes)
        {
            var curCaseType = caseTypes.Where(c => c.Id == caseTypeId).FirstOrDefault();
            if (curCaseType == null)
                return string.Empty;
            else
            {
                if (curCaseType.Parent_CaseType_Id.HasValue)
                    return string.Format("{0} - {1}",
                                         GetCaseTypeFullNameById(curCaseType.Parent_CaseType_Id.Value, caseTypes),
                                         curCaseType.Name);
                else
                    return curCaseType.Name;
            }
        }

        private IList<CaseType> GetChilds(string parentName, int parentState, IList<CaseType> subCaseTypes, bool isTakeOnlyActive = false)
        {
            var ret = new List<CaseType>();
            var newSubCaseTypes = subCaseTypes.Where(ct=> (isTakeOnlyActive? ct.IsActive == 1: true)).ToList();
            foreach (var s in newSubCaseTypes)
            {
                var newParentName = string.Format("{0} - {1}", parentName, s.Name);
                var newCT = new CaseType()
                {
                    Id = s.Id,
                    Name = newParentName,
                    IsActive = parentState,
                    Parent_CaseType_Id = s.Parent_CaseType_Id
                };
                ret.Add(newCT);
                
                if (s.SubCaseTypes.Any())
                    ret.AddRange(GetChilds(newParentName, parentState, s.SubCaseTypes.ToList(), isTakeOnlyActive));
            }

            return ret;
        }
    }
}
