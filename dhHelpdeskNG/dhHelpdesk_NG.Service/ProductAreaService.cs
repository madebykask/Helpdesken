using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Service.Utils;  
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IProductAreaService
    {
        IList<ProductArea> GetProductAreas(int customerId);
        ProductArea GetProductArea(int id);
        string GetProductAreaWithChildren(int id, string separator, string valueToReturn);
        DeleteMessage DeleteProductArea(int id);
       
        void SaveProductArea(ProductArea productArea, out IDictionary<string, string> errors);
        void Commit();
    }

    public class ProductAreaService : IProductAreaService
    {
        private readonly IProductAreaRepository _productAreaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductAreaService(
            IProductAreaRepository productAreaRepository,
            IUnitOfWork unitOfWork)
        {
            _productAreaRepository = productAreaRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<ProductArea> GetProductAreas(int customerId)
        {
            return _productAreaRepository.GetMany(x => x.Customer_Id == customerId && x.Parent_ProductArea_Id == null).OrderBy(x => x.Name).ToList();
        }
        
        public ProductArea GetProductArea(int id)
        {
            return _productAreaRepository.GetById(id);
        }

        public string GetProductAreaWithChildren(int id, string separator, string valueToReturn)
        {
            string ret = string.Empty; 

            if (id != 0)
            {
                string children = string.Empty; 
                ProductArea pa = _productAreaRepository.GetById(id);
                ret = pa.getObjectValue(valueToReturn);

                if (pa.SubProductAreas != null)
                    if (pa.SubProductAreas.Count > 0)
                        children = loopProdcuctAreas(pa.SubProductAreas.ToList(), separator, valueToReturn);

                if (!string.IsNullOrWhiteSpace(children))
                    ret += separator + children; 
            }
            return ret;
        }

        public DeleteMessage DeleteProductArea(int id)
        {
            var productArea = _productAreaRepository.GetById(id);

            if (productArea != null)
            {
                try
                {
                    _productAreaRepository.Delete(productArea);
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
                _productAreaRepository.Add(productArea);
            else
                _productAreaRepository.Update(productArea);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
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
                        ret += separator + loopProdcuctAreas(pa.SubProductAreas.ToList(), separator, valueToReturn);
            }

            return ret;
        }

    }
}
