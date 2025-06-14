﻿using System.Threading.Tasks;
using LinqLib.Operators;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.ProductArea;
    using DH.Helpdesk.BusinessData.Enums.Users;
    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.utils;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;
    using ProductAreaEntity = DH.Helpdesk.Domain.ProductArea;
    using UserGroup = DH.Helpdesk.BusinessData.Enums.Admin.Users.UserGroup;

    public interface IProductAreaService : IProductAreaNameResolver
    {
        ProductAreaEntity[] GetProductAreasForCustomer(int customerId);

        IList<ProductAreaEntity> GetTopProductAreas(int customerId, bool isOnlyActive = true);
        IList<ProductAreaEntity> GetProductAreasForSetting(int customerId, bool isOnlyActive = true);

        IList<ProductAreaEntity> GetTopProductAreasWithChilds(int customerId, bool isOnlyActive = true);

        IList<ProductAreaOverview> GetProductAreasFiltered(int customerId, int? productAreaIdToInclude,
            int? caseTypeId, UserOverview user, bool isOnlyActive = true);

        IList<ProductAreaOverview> GetTopProductAreasForUserOnCase(int customerId, int? productAreaIdToInclude, UserOverview user);
        IList<ProductAreaOverview> GetTopProductAreasForUserOnCase(int customerId, int? productAreaIdToInclude, int? caseTypeId, UserOverview user);

        IList<ProductAreaEntity> GetAllProductAreas(int customerId);

        IList<ProductAreaEntity> GetAll(int customerId);

        IList<ProductAreaEntity> GetWithHierarchy(int customerId);

        ProductAreaEntity GetProductArea(int id);
        Task<ProductAreaEntity> GetProductAreaAsync(int id);
        
        string GetProductAreaWithChildren(int id, string separator, string valueToReturn);
        string GetProductAreaChildren(int id, string separator, string valueToReturn);
        DeleteMessage DeleteProductArea(int id);

        IList<ProductArea> GetChildrenInRow(IList<ProductArea> productAreas, bool isTakeOnlyActive = false);

        void SaveProductArea(ProductAreaEntity productArea, int[] wg, int? caseTypeId, out IDictionary<string, string> errors);

        void Commit();

        /// <summary>
        /// The get product area overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ProductAreaOverview"/>.
        /// </returns>
        ProductAreaOverview GetProductAreaOverview(int id);

        /// <summary>
        /// The get same level overviews.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <param name="productAreaId">
        /// The product area id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<ProductAreaOverview> GetSameLevelOverviews(int customerId, int? productAreaId = null);

        /// <summary>
        /// The get children overviews.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <param name="parentId">
        /// The parent id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<ProductAreaOverview> GetChildrenOverviews(int customerId, int? parentId = null);

        IList<int> GetChildrenIds(int parentId);

        /// <summary>
        /// The get product area overviews.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<ProductAreaOverview> GetProductAreaOverviews(int customerId);

        IList<ProductAreaOverview> GetProductAreasOverviewWithChildren(int customerId, bool isActiveOnly = false);
        int SaveProductArea(ProductAreaOverview productArea);
    }

    public class ProductAreaService : IProductAreaService
    {
        private readonly IProductAreaRepository _productAreaRepository;
        private readonly IWorkingGroupRepository _workingGroupRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

#pragma warning disable 0618
        public ProductAreaService(
            IProductAreaRepository productAreaRepository,
            IWorkingGroupRepository workingGroupRepository,
            IUnitOfWork unitOfWork,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            _productAreaRepository = productAreaRepository;
            _workingGroupRepository = workingGroupRepository;
            _unitOfWork = unitOfWork;
            _unitOfWorkFactory = unitOfWorkFactory;
        }
#pragma warning restore 0618

        public ProductAreaEntity[] GetProductAreasForCustomer(int customerId)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<ProductAreaEntity>();
                return repository.GetAll().Where(it => it.Customer_Id == customerId).ToArray();
            }
        }

        public IList<ProductAreaEntity> GetWithHierarchy(int customerId)
        {
            return this._productAreaRepository.GetWithHierarchy(customerId);
        }

        public IList<ProductAreaEntity> GetTopProductAreas(int customerId, bool isOnlyActive = true)
        {
            return
                this._productAreaRepository.GetMany(
                    x =>
                    x.Customer_Id == customerId && x.Parent_ProductArea_Id == null
                    && ((isOnlyActive && x.IsActive != 0) || !isOnlyActive))
                    .OrderBy(x => x.Name).ToList();
        }

        public IList<ProductAreaEntity> GetProductAreasForSetting(int customerId, bool isOnlyActive = true)
        {
            return
                this._productAreaRepository.GetMany(
                    x =>
                    x.Customer_Id == customerId
                    && ((isOnlyActive && x.IsActive != 0) || !isOnlyActive)).OrderBy(x => x.Name).ToList();
        }

        public IList<ProductAreaEntity> GetTopProductAreasWithChilds(int customerId, bool isOnlyActive = true)
        {
			var res =
				this._productAreaRepository.GetManyWithSubProductAreas(
					x => x.Customer_Id == customerId && ((isOnlyActive && x.IsActive != 0) || !isOnlyActive))
                    .Where(x => x.Parent_ProductArea_Id == null)
                    .OrderBy(x => x.Name)
					.ToList();

            return res;
        }

        public IList<int> GetChildrenIds(int parentId)
        {
            return _productAreaRepository.GetChildren(parentId).Select(pa => pa.Id).ToList();
        }

        private ProductAreaOverview GetTopMostAreaForChildNew(int id, IList<ProductAreaOverview> items)
        {
            var areas = items.Where(it => it.IsActive == 1).ToDictionary(it => it.Id, it => it);

            var areaIdParentIdMap = areas.ToDictionary(kv => kv.Key, kv => kv.Value.ParentId);
            var topMostId = id;

            while (areaIdParentIdMap.ContainsKey(topMostId) && areaIdParentIdMap[topMostId].HasValue)
            {
                var removeId = topMostId;
                topMostId = areaIdParentIdMap[removeId].Value;
                areaIdParentIdMap.Remove(removeId);
            }

            if (areaIdParentIdMap.ContainsKey(topMostId) && areaIdParentIdMap[topMostId] == null && areas.ContainsKey(topMostId))
            {
                return areas[topMostId];
            }

            return null;
        }

        private ProductAreaEntity GetTopMostAreaForChild(int id)
        {
            var areas = this._productAreaRepository.GetAll()
                .Where(it => it.IsActive == 1)
                .ToDictionary(it => it.Id, it => it);

            var areaIdParentIdMap = areas.ToDictionary(kv => kv.Key, kv => kv.Value.Parent_ProductArea_Id);
            var topMostId = id;

            while (areaIdParentIdMap.ContainsKey(topMostId) && areaIdParentIdMap[topMostId].HasValue)
            {
                var removeId = topMostId;
                topMostId = areaIdParentIdMap[removeId].Value;
                areaIdParentIdMap.Remove(removeId);
            }

            if (areaIdParentIdMap.ContainsKey(topMostId) && areaIdParentIdMap[topMostId] == null && areas.ContainsKey(topMostId))
            {
                return areas[topMostId];
            }

            return null;
        }

        #region GetTopProductAreasForUserOnCase

        public IList<ProductAreaOverview> GetTopProductAreasForUserOnCase(int customerId, int? productAreaIdToInclude, UserOverview user)
        {
            return GetTopProductAreasForUserOnCase(customerId, productAreaIdToInclude, (int?)null, user);
        }

        public IList<ProductAreaOverview> GetTopProductAreasForUserOnCase(int customerId, int? productAreaIdToInclude, int? caseTypeId, UserOverview user)
        {
            var allAreas = this._productAreaRepository.GetProductAreasWithWorkingGroups(customerId, true);
            var topAreas = allAreas.Where(pa => pa.ParentId == null).ToList();

            //filter top areas by user group
            if (user.UserGroupId < (int)UserGroup.CustomerAdministrator)
            {
                var groupsMap = user.UserWorkingGroups.Where(it => it.UserRole == WorkingGroupUserPermission.ADMINSTRATOR).ToDictionary(it => it.WorkingGroup_Id, it => true);
                topAreas =
                    topAreas.Where(it => it.WorkingGroups.Count() == 0 ||
                                         it.WorkingGroups.Any(productAreaWorkingGroup => groupsMap.ContainsKey(productAreaWorkingGroup.Id))).ToList();

                var resultMap = topAreas.ToDictionary(it => it.Id, it => it);
                if (productAreaIdToInclude.HasValue)
                {
                    if (!resultMap.ContainsKey(productAreaIdToInclude.Value))
                    {
                        var productAreaToInclude = this.GetTopMostAreaForChildNew(productAreaIdToInclude.Value, allAreas);
                        if (productAreaToInclude != null && !resultMap.ContainsKey(productAreaToInclude.Id))
                        {
                            resultMap.Add(productAreaToInclude.Id, productAreaToInclude);
                        }
                    }
                }

                topAreas = resultMap.Values.OrderBy(x => x.Name).ToList();
            }

            if (topAreas.Any())
            {
                foreach (var topArea in topAreas)
                {
                    BuildProductAreaTree(topArea, allAreas);
                }

                if (caseTypeId.HasValue)
                {
                    var ctProductAreas = FilterProductAreasByCaseType(caseTypeId, topAreas);
                    if (ctProductAreas.Any())
                        return ctProductAreas; //todo: check if we shall always return filtered product areas even if empty?
                }
            }

            return topAreas.ToList();
        }

        public IList<ProductAreaOverview> GetProductAreasFiltered(int customerId, int? productAreaIdToInclude,
            int? caseTypeId, UserOverview user, bool isOnlyActive = true)
        {
            var productAreasPlain = _productAreaRepository.GetProductAreasWithWorkingGroups(customerId, isOnlyActive);
            // filter areas by user group
            if (user.UserGroupId < (int) UserGroup.CustomerAdministrator)
            {
                productAreasPlain = FilterProductAreasByUserGroup(productAreaIdToInclude, user, productAreasPlain);
            }

            // filter all by casetype
            if (caseTypeId.HasValue)
            {
                productAreasPlain = FilterProductAreasByCaseType(caseTypeId, productAreasPlain);
            }
            var topAreas = productAreasPlain.Where(pa => pa.ParentId == null).ToList();
            if (topAreas.Any())
            {
                foreach (var topArea in topAreas)
                {
                    BuildProductAreaTree(topArea, productAreasPlain);
                }
            }

            return topAreas;
        }

        private IList<ProductAreaOverview> FilterProductAreasByUserGroup(int? productAreaIdToInclude, UserOverview user,
            IList<ProductAreaOverview> productAreasPlain)
        {
            var userGroupDictionary = user.UserWorkingGroups
                .Where(it => it.UserRole == WorkingGroupUserPermission.ADMINSTRATOR)
                .ToDictionary(it => it.WorkingGroup_Id, it => true);
            return productAreasPlain
                .Where(pa =>
                    pa.WorkingGroups.Count == 0
                    || pa.WorkingGroups.Any(wg => userGroupDictionary.ContainsKey(wg.Id))
                    || (productAreaIdToInclude.HasValue && pa.Id == productAreaIdToInclude.Value))
                .ToList();
        }

        private IList<ProductAreaOverview> FilterProductAreasByCaseType(int? caseTypeId, IList<ProductAreaOverview> productAreas)
        {
            var ctProductAreas = new List<ProductAreaOverview>();
            if (caseTypeId == null)
                return ctProductAreas;

            //select top product areas that have case typeId mappings 
            var ctProductAreasIds = _productAreaRepository.GetCaseTypeProductAreas(caseTypeId.Value).Select(pa => pa.Id).ToList();
            ctProductAreas = productAreas.Where(pa => IsInProductAreaIds(pa, ctProductAreasIds)).ToList();

            // select product areas which do not have CaseTypeProduct areas mappings
            var paNoCaseTypeIds = this._productAreaRepository.GetProductAreasWithoutCaseTypes(productAreas.Select(c => c.Id).ToList());
            if (paNoCaseTypeIds.Any())
            {
                var paNoCaseType = productAreas.Where(x => paNoCaseTypeIds.Contains(x.Id)).ToList();
                ctProductAreas.AddRange(paNoCaseType.Where(p => !ctProductAreas.Select(c => c.Id).Contains(p.Id)));
            }

            return ctProductAreas;
        }


        private bool IsInProductAreaIds(ProductAreaOverview productArea, List<int> ids)
        {
            if (ids.Contains(productArea.Id))
                return true;

            if (productArea.SubProductAreas.Any())
            {
                foreach (var paChild in productArea.SubProductAreas)
                {
                    if (IsInProductAreaIds(paChild, ids))
                        return true;
                }
            }

            return false;
        }

        #endregion

        public IList<ProductAreaEntity> GetAllProductAreas(int customerId)
        {
            return this._productAreaRepository.GetMany(x => x.Customer_Id == customerId && x.Parent_ProductArea_Id == null).OrderBy(x => x.Name).ToList();
        }

        public IList<ProductAreaEntity> GetAll(int customerId)
        {
            return this._productAreaRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public ProductAreaEntity GetProductArea(int id)
        {
            return this._productAreaRepository.GetById(id);
        }

        public Task<ProductAreaEntity> GetProductAreaAsync(int id)
        {
            return _productAreaRepository.GetByIdAsync(id);
        }

        public IList<ProductAreaOverview> GetProductAreasOverviewWithChildren(int customerId, bool isActiveOnly = false)
        {
            var productAreas = this._productAreaRepository.GetProductAreasWithWorkingGroups(customerId, isActiveOnly);
            var parentItems = productAreas.Where(pa => pa.ParentId == null).OrderBy(pa => pa.Name).ToList();
            foreach(var pa in parentItems)
            {
                BuildProductAreaTree(pa, productAreas);
            }

            return parentItems;
        }

        private void BuildProductAreaTree(ProductAreaOverview parentItem, IList<ProductAreaOverview> productAreas)
        {
            var childItems = productAreas.Where(pa => pa.ParentId == parentItem.Id).OrderBy(pa => pa.Name).ToList();
            if (childItems.Any())
            {
                parentItem.SubProductAreas.AddRange(childItems);
                foreach (var childItem in childItems)
                {
                    BuildProductAreaTree(childItem, productAreas);
                }
            }
        }

        public string GetProductAreaWithChildren(int id, string separator, string valueToReturn)
        {
            string ret = string.Empty; 

            if (id != 0)
            {
                string children = string.Empty;
                ProductAreaEntity pa = this._productAreaRepository.GetById(id);
                ret = pa.getObjectValue(valueToReturn);

                if (pa.SubProductAreas != null)
                    if (pa.SubProductAreas.Count > 0)
                        children = this.loopProdcuctAreas(pa.SubProductAreas.ToList(), separator, valueToReturn);

                if (!string.IsNullOrWhiteSpace(children))
                    ret += separator + children; 
            }
            return ret;
        }

        public string GetProductAreaChildren(int id, string separator, string valueToReturn)
        {
            string ret = string.Empty;

            if (id != 0)
            {
                string children = string.Empty;
                ProductAreaEntity pa = this._productAreaRepository.GetById(id);
                ret = pa.getObjectValue(valueToReturn);

                if (pa.SubProductAreas != null)
                    if (pa.SubProductAreas.Count > 0)
                        children = this.loopProdcuctAreas(pa.SubProductAreas.ToList(), separator, valueToReturn);

                if (!string.IsNullOrWhiteSpace(children))
                {
                    ret = children;
                }
                else {
                    ret = string.Empty;
                }
                    
            }
            return ret;
        }

        public DeleteMessage DeleteProductArea(int id)
        {
            var productArea = this._productAreaRepository.GetById(id);

            if (productArea != null)
            {
                try
                {
                    productArea.CaseTypeProductAreas.Clear();
                    this._productAreaRepository.Delete(productArea);
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

        public IList<ProductArea> GetChildrenInRow(IList<ProductArea> productAreas, bool isTakeOnlyActive = false)
        {
            var childProductAreas = new List<ProductArea>();
            var parentProductAreas = productAreas.Where(pa => !pa.Parent_ProductArea_Id.HasValue && (!isTakeOnlyActive || pa.IsActive == 1)).ToList<ProductArea>();
            foreach (var p in parentProductAreas)
            {               
                childProductAreas.AddRange(GetChildren(p.Name, p.IsActive, p.SubProductAreas.ToList(), isTakeOnlyActive));
            }

            return parentProductAreas.Union(childProductAreas).OrderBy(p => p.Name).ToList();
        }

        private IList<ProductArea> GetChildren(string parentName, int parentState, IList<ProductArea> subProductAreas, bool isTakeOnlyActive = false)
        {
            var ret = new List<ProductArea>();
            var newSubProductAreas = subProductAreas.Where(pa => (!isTakeOnlyActive || pa.IsActive == 1)).ToList();
            foreach (var s in newSubProductAreas)
            {
                var newParentName = string.Format("{0} - {1}", parentName, s.Name);
                var newPA = new ProductArea()
                {
                    Id = s.Id,
                    Name = newParentName,
                    IsActive = parentState != 0 ? s.IsActive : parentState,
                    Parent_ProductArea_Id = s.Parent_ProductArea_Id
                };
                ret.Add(newPA);

                if (s.SubProductAreas.Any())
                    ret.AddRange(GetChildren(newParentName, newPA.IsActive, s.SubProductAreas.ToList(), isTakeOnlyActive));
            }

            return ret;
        }

        //public IList<ProductArea> GetParentInRow(IList<ProductArea> productAreas, bool isTakeOnlyActive = false)
        //{
        //    var allProductAreas = new List<ProductArea>();
        //    var childProductAreas = productAreas.Where(pa => pa.Parent_ProductArea_Id.HasValue && (isTakeOnlyActive ? pa.IsActive == 1 : true)).ToList();
        //    foreach (var p in childProductAreas)
        //    {
        //        p.Name = p.
        //        allProductAreas.Add(GetParents(p.Name, p.IsActive, p.SubProductAreas.ToList(), isTakeOnlyActive));
        //    }

        //    return allProductAreas.OrderBy(p => p.Name).ToList();
        //}

        //private IList<ProductArea> GetParents(string parentName, int parentState, IList<ProductArea> parentProductAreas, bool isTakeOnlyActive = false)
        //{
        //    var ret = new List<ProductArea>();
        //    var newParentProductAreas = parentProductAreas.Where(pa => (isTakeOnlyActive ? pa.IsActive == 1 : true)).ToList();
        //    foreach (var s in newParentProductAreas)
        //    {
        //        var newParentName = string.Format("{0} - {1}", s.Name, parentName);
        //        var newPA = new ProductArea()
        //        {
        //            Id = s.Id,
        //            Name = newParentName,
        //            IsActive = parentState != 0 ? s.IsActive : parentState,
        //            Parent_ProductArea_Id = s.Parent_ProductArea_Id
        //        };
        //        ret.Add(newPA);

        //        if (s.ParentProductArea != null)
        //            ret.AddRange(GetParent(newParentName, newPA.IsActive, s.SubProductAreas.ToList(), isTakeOnlyActive));
        //    }

        //    return ret;
        //}

        public void SaveProductArea(ProductAreaEntity productArea, int[] wg, int? caseTypeId, out IDictionary<string, string> errors)
        {
            if (productArea == null)
                throw new ArgumentNullException("productarea");

            errors = new Dictionary<string, string>();

            productArea.ChangedDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(productArea.Name))
                errors.Add("ProductArea.Name", "Du måste ange ett ämnesområde");

            if (productArea.IsActive == 1)
            {
                //Check if productarea has parents, if they are inactive the child can't be active
                if (productArea.Parent_ProductArea_Id.HasValue)
                {
                    var parent = GetProductArea(productArea.Parent_ProductArea_Id.Value);

                    if (parent.IsActive == 0)
                        errors.Add("ProductArea.IsActive", "Denna produktarea kan inte aktiveras, eftersom huvudnivån är inaktiv");
                }
            }

            if (caseTypeId.HasValue && caseTypeId > 0)
            {
                if (productArea.CaseTypeProductAreas != null)
                {
                    productArea.CaseTypeProductAreas.Clear();
                    productArea.CaseTypeProductAreas.Add(new CaseTypeProductArea
                    {
                        CaseType_Id = caseTypeId.Value
                    });
                }
                else
                {
                    productArea.CaseTypeProductAreas = new List<CaseTypeProductArea>
                    {
                        new CaseTypeProductArea
                        {
                            CaseType_Id = caseTypeId.Value
                        }
                    };
                }
            }
            else
            {
                if (productArea.CaseTypeProductAreas != null && productArea.CaseTypeProductAreas.Count > 0)
                {
                    productArea.CaseTypeProductAreas.Clear();
                }
            }

            if (productArea.IsActive == 0)
            {
                //Check if productarea has childs and inactivate the child 
                var children = GetProductAreaChildren(productArea.Id, ",", "Id");
                if (!string.IsNullOrEmpty(children))
                {
                    List<string> listOfChilds = new List<string>(children.Split(',')).ToList();
                    List<int> listOfChildsId = listOfChilds.Select(s => int.Parse(s)).ToList();

                    foreach (var child in listOfChildsId)
                    {
                        var childProductArea = GetProductArea(child);
                        if (childProductArea.IsActive == 1)
                            childProductArea.IsActive = 0;

                        SaveProductArea(childProductArea, null, 0, out errors);
                    }
                }

            }
            if (productArea.WorkingGroups != null)
                foreach (var delete in productArea.WorkingGroups.ToList())
                    productArea.WorkingGroups.Remove(delete);
            else
                productArea.WorkingGroups = new List<WorkingGroupEntity>();

            if (wg != null)
            {
                foreach (int id in wg)
                {
                    var w = this._workingGroupRepository.GetById(id);
                    if (w != null)
                        productArea.WorkingGroups.Add(w);
                }
            }

            if (productArea.Id == 0)
                this._productAreaRepository.Add(productArea);
            else
                this._productAreaRepository.Update(productArea);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        /// <summary>
        /// The get product area overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ProductAreaOverview"/>.
        /// </returns>
        public ProductAreaOverview GetProductAreaOverview(int id)
        {
            return this._productAreaRepository.GetProductAreaOverview(id);
        }

        /// <summary>
        /// The get same level overviews.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <param name="productAreaId">
        /// The product area id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<ProductAreaOverview> GetSameLevelOverviews(int customerId, int? productAreaId = null)
        {
            return this._productAreaRepository.GetSameLevelOverviews(customerId, productAreaId);
        }

        /// <summary>
        /// The get children overviews.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <param name="parentId">
        /// The parent id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<ProductAreaOverview> GetChildrenOverviews(int customerId, int? parentId = null)
        {
            return this._productAreaRepository.GetChildrenOverviews(customerId, parentId);
        }

        /// <summary>
        /// The get product area overviews.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<ProductAreaOverview> GetProductAreaOverviews(int customerId)
        {
            return this._productAreaRepository.GetProductAreaOverviews(customerId);
        }

        public int SaveProductArea(ProductAreaOverview productArea)
        {
            return this._productAreaRepository.SaveProductArea(productArea);
        }

        private string loopProdcuctAreas(IList<ProductAreaEntity> pal, string separator, string valueToReturn)
        {
            string ret = string.Empty;

            foreach (var pa in pal)
            {
                if (string.IsNullOrWhiteSpace(ret))
                    ret += pa.getObjectValue(valueToReturn);
                else
                    ret += separator + pa.getObjectValue(valueToReturn);

                if (pa.SubProductAreas != null)
                    if (pa.SubProductAreas.Count > 0)
                        ret += separator + this.loopProdcuctAreas(pa.SubProductAreas.ToList(), separator, valueToReturn);
            }

            return ret;
        }

        /// <summary>
        /// Returns list of parent product categories including supplyed category by productAreaId
        /// </summary>
        /// <param name="productAreaId"></param>
        /// <returns></returns>
        public IEnumerable<string> GetParentPath(int productAreaId, int customerId)
        {
            if (this.productAreaCache == null || this.cachiedForCustomer != customerId)
            {
                this.productAreaCache = this.GetProductAreasForCustomer(customerId).ToDictionary(it => it.Id, it => it);
                this.cachiedForCustomer = customerId;
            }

            var recursionsMax = 10;
            int? lookingProductAreaId = productAreaId;
            var res = new List<string>();
            while (lookingProductAreaId.HasValue && this.productAreaCache.ContainsKey(lookingProductAreaId.Value) && recursionsMax-- > 0)
            {
                res.Add(this.productAreaCache[lookingProductAreaId.Value].Name);                               
                lookingProductAreaId = this.productAreaCache[lookingProductAreaId.Value].Parent_ProductArea_Id;
            }

            return res.AsEnumerable().Reverse();
        }

        public IEnumerable<string> GetParentPathOnExternalPage(int productAreaId, int customerId, out bool checkShowOnExtenal)
        {
            checkShowOnExtenal = true;
            if (this.productAreaCache == null || this.cachiedForCustomer != customerId)
            {
                this.productAreaCache = this.GetProductAreasForCustomer(customerId).ToDictionary(it => it.Id, it => it);
                this.cachiedForCustomer = customerId;
            }

            var recursionsMax = 10;
            int? lookingProductAreaId = productAreaId;
            var res = new List<string>();
            while (lookingProductAreaId.HasValue && this.productAreaCache.ContainsKey(lookingProductAreaId.Value) && recursionsMax-- > 0)
            {
                res.Add(this.productAreaCache[lookingProductAreaId.Value].Name);

                //if (this.productAreaCache[lookingProductAreaId.Value].ShowOnExternalPage == 0)
                    //Hide this for next release #57742
               if (this.productAreaCache[lookingProductAreaId.Value].ShowOnExtPageCases == 0)
                    checkShowOnExtenal = false;

                lookingProductAreaId = this.productAreaCache[lookingProductAreaId.Value].Parent_ProductArea_Id;
            }

            return res.AsQueryable().Reverse().ToArray();
        }

        private Dictionary<int, ProductAreaEntity> productAreaCache;

        private int cachiedForCustomer;
    }
}
