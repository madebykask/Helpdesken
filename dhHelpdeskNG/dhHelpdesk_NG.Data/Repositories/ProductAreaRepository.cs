using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain;
    using ProductAreaEntity = DH.Helpdesk.Domain.ProductArea;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Data;
    using System;

    #region PRODUCTAREA

    /// <summary>
    /// The ProductAreaRepository interface.
    /// </summary>
    public interface IProductAreaRepository : IRepository<ProductAreaEntity>
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

        /// <summary>
        /// The get product area overviews.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        IEnumerable<ProductAreaOverview> GetProductAreaOverviews(int customerId);

        IList<ProductAreaOverview> GetProductAreasWithWorkingGroups(int customerId, bool isActiveOnly);

        int SaveProductArea(ProductAreaOverview productArea);


        IList<ProductAreaEntity> GetWithHierarchy(int customerId);
    }

    /// <summary>
    /// The product area repository.
    /// </summary>
    public class ProductAreaRepository : RepositoryBase<ProductAreaEntity>, IProductAreaRepository
    {
        /// <summary>
        /// The product area entity to business model mapper.
        /// </summary>
        private readonly IEntityToBusinessModelMapper<ProductAreaEntity, ProductAreaOverview> productAreaEntityToBusinessModelMapper;

        private readonly IBusinessModelToEntityMapper<ProductAreaOverview, ProductAreaEntity> toEntityMapper;


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
            IEntityToBusinessModelMapper<ProductAreaEntity, ProductAreaOverview> productAreaEntityToBusinessModelMapper,
            IBusinessModelToEntityMapper<ProductAreaOverview, ProductAreaEntity> toEntityMapper)
            : base(databaseFactory)
        {
            this.productAreaEntityToBusinessModelMapper = productAreaEntityToBusinessModelMapper;
            this.toEntityMapper = toEntityMapper;
        }



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
            var entities = this.Table
                .Where(p => p.Id == id)
                .ToList();

            return entities
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


        public IList<ProductAreaEntity> GetWithHierarchy(int customerId)
        {
            string sql = "SELECT[Id] AS Id, isnull(dbo.GetHierarchy(Id, 'tblProductArea'), '') AS Name, ";
            sql += "ProductAreaGUID AS Guid, ";
            sql += "cast(0 as bit) AS[Selected] ";
            sql += "FROM tblProductArea AS[Extent1] WHERE(Customer_Id is null or Customer_Id = 1) ORDER BY Name";

            List<ProductAreaEntity> l = new List<ProductAreaEntity>();
            DataTable dt = null;
            string con = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;

         

            using (var connection = new SqlConnection(con))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(reader);

                    foreach (DataRow row in dt.Rows)
                    {
                        ProductAreaEntity p = new ProductAreaEntity
                        {
                            Id = Convert.ToInt32(row["Id"].ToString()),
                            Name = row["Name"].ToString()

                        };

                        l.Add(p);

                    }

                }

            }

            return l.ToList();

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
            var entities = this.Table
                .Where(p => p.Customer_Id == customerId && p.Parent_ProductArea_Id == parentId)
                .OrderBy(p => p.Name)
                .ToList();

            return entities.Select(this.productAreaEntityToBusinessModelMapper.Map);
        }

        public IList<ProductAreaOverview> GetProductAreasWithWorkingGroups(int customerId, bool isActiveOnly)
        {
            //note please do not use mapper since more fields will be read from database ... for correct sql projection with required fields 
            var productAreas = 
                from pa in DataContext.ProductAreas
                where pa.Customer_Id == customerId && 
                      (!isActiveOnly || pa.IsActive > 0)
                select new ProductAreaOverview
                {
                    Id = pa.Id,
                    ParentId = pa.Parent_ProductArea_Id,
                    Name = pa.Name,
                    IsActive = pa.IsActive,
                    Description = pa.Description,
                    WorkingGroups =
                        pa.WorkingGroups.Select(wg => new WorkingGroupOverview
                        {
                            Id = wg.Id,
                            Code = wg.Code,
                            WorkingGroupName = wg.WorkingGroupName
                        }).ToList()
                };

            return productAreas.ToList();

        }

        /// <summary>
        /// The get product area overviews.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<ProductAreaOverview> GetProductAreaOverviews(int customerId)
        {
            var entities = this.Table
                .Where(p => p.Customer_Id == customerId)
                .OrderBy(p => p.Name)
                .ToList();

            return entities
                .Select(this.productAreaEntityToBusinessModelMapper.Map);
        }

        public int SaveProductArea(ProductAreaOverview productArea)
        {
            ProductAreaEntity entity;
            if (productArea.Id > 0)
            {
                entity = this.DataContext.ProductAreas.Find(productArea.Id);
                this.toEntityMapper.Map(productArea, entity);
            }
            else
            {
                entity = new ProductAreaEntity();
                this.toEntityMapper.Map(productArea, entity);
                this.DataContext.ProductAreas.Add(entity);
            }

            this.Commit();
            return entity.Id;
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
