namespace DH.Helpdesk.Dal.Repositories
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.Dal.Infrastructure;
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
    }

    /// <summary>
    /// The product area repository.
    /// </summary>
    public class ProductAreaRepository : RepositoryBase<ProductArea>, IProductAreaRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductAreaRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public ProductAreaRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
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
                .Select(p => new ProductAreaOverview()
                                 {
                                     Id = p.Id,
                                     Name = p.Name
                                 })
                .FirstOrDefault();
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
