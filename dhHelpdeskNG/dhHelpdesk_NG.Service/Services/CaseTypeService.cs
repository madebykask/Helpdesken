using DH.Helpdesk.BusinessData.Models.Case.Output;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Data.Entity;
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

        IList<CaseType> GetCaseTypesForSetting(int customerId, bool isTakeOnlyActive = false);
        IList<CaseType> GetAllCaseTypes(int customerId, bool isTakeOnlyActive = false, bool includeSubType = false);
        IList<CaseTypeOverview> GetCaseTypesRelatedFields(int customerId, bool isExtnernalSiteOnly = false, bool isTakeOnlyActive = false);

        CaseType GetCaseType(int id);

        DeleteMessage DeleteCaseType(int id);

        int GetDefaultId(int customerId); 

        void SaveCaseType(CaseType caseType, out IDictionary<string, string> errors);

        void Commit();

        IList<ItemOverview> GetOverviews(int customerId);

        IList<ItemOverview> GetOverviews(int customerId, IEnumerable<int> caseTypesIds);

        IList<CaseTypeOverview> GetCaseTypesOverviewWithChildren(int customerId, bool activeOnly = false);

        IList<CaseType> GetChildrenInRow(IList<CaseType> caseTypes, bool isTakeOnlyActive = false);
        IList<int> GetChildrenIds(int caseTypeId);

        string GetCaseTypeFullName(int caseTypeId);
    }

    public class CaseTypeService : ICaseTypeService
    {
        private readonly ICaseTypeRepository _caseTypeRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618
        private readonly ICaseRepository _caseRepository;

#pragma warning disable 0618
        public CaseTypeService(
            ICaseTypeRepository caseTypeRepository,
            ICaseRepository caseRepository,
            IUnitOfWork unitOfWork)            
        {
            this._caseTypeRepository = caseTypeRepository;
            this._unitOfWork = unitOfWork;
            this._caseRepository = caseRepository;
        }
#pragma warning restore 0618

        public IList<CaseType> GetCaseTypes(int customerId, bool isTakeOnlyActive = false)
        {
            var query = _caseTypeRepository.GetManyWithSubCaseTypes(o => o.Customer_Id == customerId)
                .Where(o => o.Parent_CaseType_Id == null);
            if (isTakeOnlyActive)
            {
                query = query.Where(it => it.IsActive == 1 && it.Selectable == 1);
            }

            return query.OrderBy(x => x.Name).ToList();
        }

        public IList<CaseType> GetCaseTypesForSetting(int customerId, bool isTakeOnlyActive = false)
        {
            var query = this._caseTypeRepository.GetMany(
                x => x.Customer_Id == customerId);
            if (isTakeOnlyActive)
            {
                query = query.Where(it => it.IsActive == 1 && it.Selectable == 1);
            }

            return query.OrderBy(x => x.Name).ToList();
        }

        public IList<CaseTypeOverview> GetCaseTypesRelatedFields(int customerId, bool isExtnernalSiteOnly = false, bool isTakeOnlyActive = false)
        {
            var query = _caseTypeRepository.GetMany(x => x.Customer_Id == customerId);

            if (isTakeOnlyActive)
            {
                query = query.Where(it => it.IsActive == 1 && it.Selectable == 1);
            }

            if (isExtnernalSiteOnly)
            {
                query = query.Where(it => it.ShowOnExternalPage != 0);
            }

            return 
                query.Select(x => new CaseTypeOverview()
                {
                    Id = x.Id,
                    RelatedField = x.RelatedField
                }).ToList();
        }

        public IList<CaseType> GetAllCaseTypes(int customerId, bool isTakeOnlyActive = false, bool includeSubType = false)
        {
            var query = includeSubType ? _caseTypeRepository.GetManyWithSubCaseTypes(x => x.Customer_Id == customerId) :
                                         _caseTypeRepository.GetMany(x => x.Customer_Id == customerId).AsQueryable();
            if (isTakeOnlyActive)
            {
                query = query.Where(it => it.IsActive == 1 && it.Selectable == 1);
            }

            return query.OrderBy(x => x.Name).ToList();
        }

        public CaseType GetCaseType(int id)
        {
            return this._caseTypeRepository.GetCaseTypeFull(id);
        }

        public int GetDefaultId(int customerId)
        {
            var r = this._caseTypeRepository.GetMany(x => x.Customer_Id == customerId && x.IsDefault == 1).FirstOrDefault();
            if (r == null)
            {
                return 0;
            }

            return r.Id;
        }

        public DeleteMessage DeleteCaseType(int id)
        {
            var caseType = this._caseTypeRepository.GetById(id);
            
            if (caseType != null && !this._caseRepository.GetCasesIdsByType(id).Any())
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
                this._caseTypeRepository.Add(caseType);
            }
            else
            {
                this._caseTypeRepository.Update(caseType);
            }

            if (caseType.IsDefault == 1)
            {
                this._caseTypeRepository.ResetDefault(caseType.Id, caseType.Customer_Id);
            }

            if (caseType.IsEMailDefault == 1)
            {
                this._caseTypeRepository.ResetEmailDefault(caseType.Id, caseType.Customer_Id);
            }

            if (errors.Count == 0)
            {
                this.Commit();
            }
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public IList<ItemOverview> GetOverviews(int customerId)
        {
            return this._caseTypeRepository.GetOverviews(customerId);
        }

        public IList<ItemOverview> GetOverviews(int customerId, IEnumerable<int> caseTypesIds)
        {
            return this._caseTypeRepository.GetOverviews(customerId, caseTypesIds);
        }

        public IList<CaseTypeOverview> GetCaseTypesOverviewWithChildren(int customerId, bool activeOnly = false)
        {
            var allItems =
                this._caseTypeRepository.GetMany(
                        x => x.Customer_Id == customerId && (!activeOnly || (x.IsActive == 1 && x.Selectable == 1)))
                    .AsQueryable()
                    .Select(x => new CaseTypeOverview
                    {
                        Id = x.Id,
                        ParentId = x.Parent_CaseType_Id,
                        Name = x.Name,
                        ParentName = x.ParentCaseType != null ? x.ParentCaseType.Name : null,
                        ShowOnExternalPage = x.ShowOnExternalPage,
                        ShowOnExtPageCases = x.ShowOnExtPageCases,
                        IsActive = x.IsActive,
                        Selectable = x.Selectable,
                        WorkingGroupId = x.WorkingGroup_Id,
                        AdministratorId = x.User_Id
                    })
                    .ToList();

            var parentItems = allItems.Where(o => o.ParentId == null).OrderBy(o => o.Name).ToList();
            
            foreach (var parentItem in parentItems)
            {
                BuildCaseTypesOverviewTree(parentItem, allItems);
            }

            return parentItems;
        }

        private void BuildCaseTypesOverviewTree(CaseTypeOverview parentCaseType, List<CaseTypeOverview> items)
        {
            var children = items.Where(o => o.ParentId == parentCaseType.Id).OrderBy(o => o.Name).ToList();

            if (children.Any())
            {
                parentCaseType.SubCaseTypes.AddRange(children);
                foreach (var child in children)
                {
                    BuildCaseTypesOverviewTree(child, items);
                }
            }
        }

        public IList<CaseType> GetChildrenInRow(IList<CaseType> caseTypes, bool isTakeOnlyActive = false)
        {
            var childCaseTypes = new List<CaseType>();
            var parentCaseTypes = caseTypes.Where(ct=> !ct.Parent_CaseType_Id.HasValue && (!isTakeOnlyActive || ct.IsActive == 1)).ToList();
            foreach (var p in parentCaseTypes)
            {
                childCaseTypes.AddRange(GetChilds(p.Name, p.IsActive, p.SubCaseTypes.ToList(), isTakeOnlyActive));
            }

            return parentCaseTypes.Union(childCaseTypes).OrderBy(c => c.Name).ToList();
        }

        public IList<int> GetChildrenIds(int caseTypeId)
        {
            return _caseTypeRepository.GetChildren(caseTypeId);
        }

        public string GetCaseTypeFullName(int caseTypeId)
        {
            var allCaseTypes = this._caseTypeRepository.GetAll().ToList();            
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
            var newSubCaseTypes = subCaseTypes.Where(ct=> (!isTakeOnlyActive || ct.IsActive == 1)).ToList();
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
