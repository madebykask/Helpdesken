namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain;

    #region PRODUCTAREA

    /// <summary>
    /// The ProductAreaRepository interface.
    /// </summary>
    public interface IProductAreaRepository : IRepository<ProductArea>
    {
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
        /// The <see cref="IEnumerable"/>.
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
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        IEnumerable<ProductAreaOverview> GetChildrenOverviews(int customerId, int? parentId = null);
    }

    /// <summary>
    /// The product area repository.
    /// </summary>
    public class ProductAreaRepository : RepositoryBase<ProductArea>, IProductAreaRepository
    {
        /// <summary>
        /// The product area entity to business model mapper.
        /// </summary>
        private readonly IEntityToBusinessModelMapper<ProductArea, ProductAreaOverview> productAreaEntityToBusinessModelMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductAreaRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        /// <param name="productAreaEntityToBusinessModelMapper">
        /// The product area entity to business model mapper.
        /// </param>
        public ProductAreaRepository(
            IDatabaseFactory databaseFactory, 
            IEntityToBusinessModelMapper<ProductArea, ProductAreaOverview> productAreaEntityToBusinessModelMapper)
            : base(databaseFactory)
        {
            this.productAreaEntityToBusinessModelMapper = productAreaEntityToBusinessModelMapper;
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
            return this.GetAll()
                .Where(p => p.Id == id)
                .Select(this.productAreaEntityToBusinessModelMapper.Map)
                .FirstOrDefault();
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
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<ProductAreaOverview> GetSameLevelOverviews(int customerId, int? productAreaId = null)
        {
            int? parentId = null;
            if (productAreaId.HasValue)
            {
                var productArea = this.GetProductAreaOverview(productAreaId.Value);
                if (productArea != null)
                {
                    parentId = productArea.ParentId;
                }
            }

            return this.GetChildrenOverviews(customerId, parentId);
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
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<ProductAreaOverview> GetChildrenOverviews(int customerId, int? parentId = null)
        {
            return this.GetAll()
                .Where(p => p.Customer_Id == customerId && p.Parent_ProductArea_Id == parentId)
                .Select(this.productAreaEntityToBusinessModelMapper.Map)
                .OrderBy(p => p.Name);            
        }
    }

    #endregion

    #region PRODUCTAREAQUESTION

    public interface IProductAreaQuestionRepository : IRepository<ProductAreaQuestion>
    {
    }

    public class ProductAreaQuestionRepository : RepositoryBase<ProductAreaQuestion>, IProductAreaQuestionRepository
    {
        public ProductAreaQuestionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region PRODUCTAREAQUESTIONVERSION

    public interface IProductAreaQuestionVersionRepository : IRepository<ProductAreaQuestionVersion>
    {
    }

    public class ProductAreaQuestionVersionRepository : RepositoryBase<ProductAreaQuestionVersion>, IProductAreaQuestionVersionRepository
    {
        public ProductAreaQuestionVersionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    //#region PRODUCTAREAWORKINGGROUP

    //public interface IProductAreaWorkingGroupRepository : IRepository<ProductAreaWorkingGroup>
    //{
    //}

    //public class ProductAreaWorkingGroupRepository : RepositoryBase<ProductAreaWorkingGroup>, IProductAreaWorkingGroupRepository
    //{
    //    public ProductAreaWorkingGroupRepository(IDatabaseFactory databaseFactory)
    //        : base(databaseFactory)
    //    {
    //    }
    //}

    //#endregion
}
