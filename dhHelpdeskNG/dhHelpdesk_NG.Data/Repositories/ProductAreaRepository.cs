using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using System.Collections.Generic;
using System.Linq;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region PRODUCTAREA

    public interface IProductAreaRepository : IRepository<ProductArea>
    {
    }

    public class ProductAreaRepository : RepositoryBase<ProductArea>, IProductAreaRepository
    {
        public ProductAreaRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
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
