namespace DH.Helpdesk.Web.Areas.Licenses.Models.Products
{
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;

    public sealed class ProductsIndexModel : BaseIndexModel
    {
        public override IndexModelType Type
        {
            get
            {
                return IndexModelType.Products;
            }
        }
    }
}