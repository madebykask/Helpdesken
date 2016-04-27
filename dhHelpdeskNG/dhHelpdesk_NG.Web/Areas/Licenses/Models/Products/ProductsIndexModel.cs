namespace DH.Helpdesk.Web.Areas.Licenses.Models.Products
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;

    public sealed class ProductsIndexModel : BaseIndexModel
    {
        public ProductsIndexModel(
                MultiSelectList regions, 
                MultiSelectList departments,
                MultiSelectList products)
        {
            this.Departments = departments;
            this.Regions = regions;
            this.Products = products;
        }

        public ProductsIndexModel()
        {
            this.RegionIds = new int[0];
            this.DepartmentIds = new int[0];
            this.ProductIds = new int[0];
        }

        public override EntityModelType Type
        {
            get
            {
                return EntityModelType.Products;
            }
        }

        [NotNull]
        public MultiSelectList Regions { get; private set; }

        [NotNull]
        public MultiSelectList Departments { get; private set; }

        [NotNull]
        public MultiSelectList Products { get; private set; }

        [NotNull]
        public int[] RegionIds { get; set; }
        
        [NotNull]
        public int[] DepartmentIds { get; set; }

        [NotNull]
        public int[] ProductIds { get; set; }

        public ProductsFilterModel GetFilter()
        {
            return new ProductsFilterModel(this.RegionIds, this.DepartmentIds, this.ProductIds);
        }
    }
}