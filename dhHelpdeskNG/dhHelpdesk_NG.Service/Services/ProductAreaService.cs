namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.utils;

    public interface IProductAreaService
    {
        IList<ProductArea> GetProductAreas(int customerId);

        ProductArea GetProductArea(int id);

        string GetProductAreaWithChildren(int id, string separator, string valueToReturn);

        DeleteMessage DeleteProductArea(int id);
       
        void SaveProductArea(ProductArea productArea, out IDictionary<string, string> errors);

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

        private readonly IUnitOfWork unitOfWork;

        public ProductAreaService(
            IProductAreaRepository productAreaRepository,
            IUnitOfWork unitOfWork)
        {
            this.productAreaRepository = productAreaRepository;
            this.unitOfWork = unitOfWork;
        }

        public IList<ProductArea> GetProductAreas(int customerId)
        {
            return this.productAreaRepository.GetMany(x => x.Customer_Id == customerId && x.Parent_ProductArea_Id == null).OrderBy(x => x.Name).ToList();
        }
        
        public ProductArea GetProductArea(int id)
        {
            return this.productAreaRepository.GetById(id);
        }

        public string GetProductAreaWithChildren(int id, string separator, string valueToReturn)
        {
            string ret = string.Empty; 

            if (id != 0)
            {
                string children = string.Empty; 
                ProductArea pa = this.productAreaRepository.GetById(id);
                ret = pa.getObjectValue(valueToReturn);

                if (pa.SubProductAreas != null)
                    if (pa.SubProductAreas.Count > 0)
                        children = this.loopProdcuctAreas(pa.SubProductAreas.ToList(), separator, valueToReturn);

                if (!string.IsNullOrWhiteSpace(children))
                    ret += separator + children; 
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

        public void SaveProductArea(ProductArea productArea, out IDictionary<string, string> errors)
        {
            if (productArea == null)
                throw new ArgumentNullException("productarea");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(productArea.Name))
                errors.Add("ProductArea.Name", "Du måste ange ett ämnesområde");

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

        private string loopProdcuctAreas(IList<ProductArea> pal, string separator, string valueToReturn)
        {
            string ret = string.Empty;

            foreach (ProductArea pa in pal)
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

    }
}
