namespace DH.Helpdesk.Web.Areas.Licenses.Models.Products
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;

    public sealed class ProductsIndexModel : BaseIndexModel
    {
        public ProductsIndexModel(
                MultiSelectList regions, 
                MultiSelectList departments)
        {
            this.Departments = departments;
            this.Regions = regions;
        }

        public ProductsIndexModel()
        {
            this.RegionIds = new int[0];
            this.DepartmentIds = new int[0];
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
        public int[] RegionIds { get; set; }
        
        [NotNull]
        public int[] DepartmentIds { get; set; }

        public ProductsFilterModel GetFilter()
        {
            return new ProductsFilterModel(this.RegionIds, this.DepartmentIds);
        }
    }
}