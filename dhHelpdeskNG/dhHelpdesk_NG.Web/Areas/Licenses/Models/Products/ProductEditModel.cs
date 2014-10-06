namespace DH.Helpdesk.Web.Areas.Licenses.Models.Products
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Products;

    public sealed class ProductEditModel
    {
        public ProductEditModel(ProductData data)
        {
            this.Data = data;
        }

        public ProductEditModel()
        {            
        }

        public ProductData Data { get; set; }
    }
}