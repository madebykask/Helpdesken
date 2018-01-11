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

        IList<ProductAreaEntity> GetTopProductAreasForUser(int customerId, UserOverview user, bool isOnlyActive = true);

        IList<ProductAreaEntity> GetTopProductAreasForUserOnCase(
            int customerId,
            int? productAreaIdToInclude,
            UserOverview user);

        IList<ProductAreaEntity> GetAllProductAreas(int customerId);

        IList<ProductAreaEntity> GetAll(int customerId);

        IList<ProductAreaEntity> GetWithHierarchy(int customerId);

        ProductAreaEntity GetProductArea(int id);

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

        int SaveProductArea(ProductAreaOverview productArea);
    }

    public class ProductAreaService : IProductAreaService
    {
        private readonly IProductAreaRepository productAreaRepository;
        private readonly IWorkingGroupRepository workingGroupRepository;

        private readonly IUnitOfWork unitOfWork;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        

        public ProductAreaService(
            IProductAreaRepository productAreaRepository,
            IWorkingGroupRepository workingGroupRepository,
            IUnitOfWork unitOfWork,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.productAreaRepository = productAreaRepository;
            this.workingGroupRepository = workingGroupRepository;
            this.unitOfWork = unitOfWork;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public ProductAreaEntity[] GetProductAreasForCustomer(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<ProductAreaEntity>();
                return repository.GetAll().Where(it => it.Customer_Id == customerId).ToArray();
            }
        }


        public IList<ProductAreaEntity> GetWithHierarchy(int customerId)
        {
            return this.productAreaRepository.GetWithHierarchy(customerId);
        }

        public IList<ProductAreaEntity> GetTopProductAreas(int customerId, bool isOnlyActive = true)
        {
            return
                this.productAreaRepository.GetMany(
                    x =>
                    x.Customer_Id == customerId && x.Parent_ProductArea_Id == null
                    && ((isOnlyActive && x.IsActive != 0) || !isOnlyActive)).OrderBy(x => x.Name).ToList();
        }

        public IList<ProductAreaEntity> GetProductAreasForSetting(int customerId, bool isOnlyActive = true)
        {
            return
                this.productAreaRepository.GetMany(
                    x =>
                    x.Customer_Id == customerId
                    && ((isOnlyActive && x.IsActive != 0) || !isOnlyActive)).OrderBy(x => x.Name).ToList();
        }

        public IList<ProductAreaEntity> GetTopProductAreasForUser(int customerId, UserOverview user, bool isOnlyActive = true)
        {
            var res =
                this.productAreaRepository.GetMany(
                    x =>
                    x.Customer_Id == customerId && x.Parent_ProductArea_Id == null
                    && ((isOnlyActive && x.IsActive != 0) || !isOnlyActive));

            return res.OrderBy(x => x.Name).ToList();
        }

        private ProductAreaEntity GetTopMostAreaForChild(int id)
        {
            var areas = this.productAreaRepository.GetAll()
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

        public IList<ProductAreaEntity> GetTopProductAreasForUserOnCase(int customerId, int? productAreaIdToInclude, UserOverview user)
        {
            var res =
                this.productAreaRepository.GetMany(
                    x =>
                    x.Customer_Id == customerId && x.Parent_ProductArea_Id == null
                    && x.IsActive != 0);

            if (user.UserGroupId < (int)UserGroup.CustomerAdministrator)
            { 
                var groupsMap = user.UserWorkingGroups.Where(it => it.UserRole == WorkingGroupUserPermission.ADMINSTRATOR).ToDictionary(it => it.WorkingGroup_Id, it => true);
                res = res.Where(
                    it => it.WorkingGroups.Count == 0 || it.WorkingGroups.Any(productAreaWorkingGroup => groupsMap.ContainsKey(productAreaWorkingGroup.Id)));

                var resultMap = res.ToDictionary(it => it.Id, it => it);
                if (productAreaIdToInclude.HasValue)
                {
                    if (!resultMap.ContainsKey(productAreaIdToInclude.Value))
                    {
                        var productAreaToInclude = this.GetTopMostAreaForChild(productAreaIdToInclude.Value);
                        if (productAreaToInclude != null && !resultMap.ContainsKey(productAreaToInclude.Id))
                        {
                            resultMap.Add(productAreaToInclude.Id, productAreaToInclude);
                        }
                    }
                }

                return resultMap.Values.OrderBy(x => x.Name).ToList();
            }
          
            return res.OrderBy(x => x.Name).ToList();
        }

        public IList<ProductAreaEntity> GetAllProductAreas(int customerId)
        {
            return this.productAreaRepository.GetMany(x => x.Customer_Id == customerId && x.Parent_ProductArea_Id == null).OrderBy(x => x.Name).ToList();
        }

        public IList<ProductAreaEntity> GetAll(int customerId)
        {
            return this.productAreaRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public ProductAreaEntity GetProductArea(int id)
        {
            return this.productAreaRepository.GetById(id);
        }

        public string GetProductAreaWithChildren(int id, string separator, string valueToReturn)
        {
            string ret = string.Empty; 

            if (id != 0)
            {
                string children = string.Empty;
                ProductAreaEntity pa = this.productAreaRepository.GetById(id);
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
                ProductAreaEntity pa = this.productAreaRepository.GetById(id);
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
            var productArea = this.productAreaRepository.GetById(id);

            if (productArea != null)
            {
                try
                {
                    productArea.CaseTypeProductAreas.Clear();
                    this.productAreaRepository.Delete(productArea);
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
            var parentProductAreas = productAreas.Where(pa => !pa.Parent_ProductArea_Id.HasValue && (isTakeOnlyActive ? pa.IsActive == 1 : true)).ToList<ProductArea>();
            foreach (var p in parentProductAreas)
            {               
                childProductAreas.AddRange(GetChildren(p.Name, p.IsActive, p.SubProductAreas.ToList(), isTakeOnlyActive));
            }

            return parentProductAreas.Union(childProductAreas).OrderBy(p => p.Name).ToList();
        }

        private IList<ProductArea> GetChildren(string parentName, int parentState, IList<ProductArea> subProductAreas, bool isTakeOnlyActive = false)
        {
            var ret = new List<ProductArea>();
            var newSubProductAreas = subProductAreas.Where(pa => (isTakeOnlyActive ? pa.IsActive == 1 : true)).ToList();
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
                if (productArea.CaseTypeProductAreas.Count > 0)
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
                    var w = this.workingGroupRepository.GetById(id);
                    if (w != null)
                        productArea.WorkingGroups.Add(w);
                }
            }

            if (productArea.Id == 0)
                this.productAreaRepository.Add(productArea);
            else
                this.productAreaRepository.Update(productArea);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this.unitOfWork.Commit();
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
            return this.productAreaRepository.GetProductAreaOverview(id);
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
            return this.productAreaRepository.GetSameLevelOverviews(customerId, productAreaId);
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
            return this.productAreaRepository.GetChildrenOverviews(customerId, parentId);
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
            return this.productAreaRepository.GetProductAreaOverviews(customerId);
        }

        public int SaveProductArea(ProductAreaOverview productArea)
        {
            return this.productAreaRepository.SaveProductArea(productArea);
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

            return res.AsQueryable().Reverse().ToArray();
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
