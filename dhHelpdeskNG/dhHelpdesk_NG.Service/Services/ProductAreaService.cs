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
    }

    public class ProductAreaService : IProductAreaService
    {
        private readonly IProductAreaRepository _productAreaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductAreaService(
            IProductAreaRepository productAreaRepository,
            IUnitOfWork unitOfWork)
        {
            this._productAreaRepository = productAreaRepository;
            this._unitOfWork = unitOfWork;
        }

        public IList<ProductArea> GetProductAreas(int customerId)
        {
            return this._productAreaRepository.GetMany(x => x.Customer_Id == customerId && x.Parent_ProductArea_Id == null).OrderBy(x => x.Name).ToList();
        }
        
        public ProductArea GetProductArea(int id)
        {
            return this._productAreaRepository.GetById(id);
        }

        public string GetProductAreaWithChildren(int id, string separator, string valueToReturn)
        {
            string ret = string.Empty; 

            if (id != 0)
            {
                string children = string.Empty; 
                ProductArea pa = this._productAreaRepository.GetById(id);
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
            var productArea = this._productAreaRepository.GetById(id);

            if (productArea != null)
            {
                try
                {
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

        public void SaveProductArea(ProductArea productArea, out IDictionary<string, string> errors)
        {
            if (productArea == null)
                throw new ArgumentNullException("productarea");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(productArea.Name))
                errors.Add("ProductArea.Name", "Du måste ange ett ämnesområde");

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
