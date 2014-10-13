namespace DH.Helpdesk.Web.Areas.Licenses.Models.Products
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProductsFilterModel
    {
        public ProductsFilterModel(
                int[] regionIds, 
                int[] departmentIds)
        {
            this.DepartmentIds = departmentIds;
            this.RegionIds = regionIds;
        }

        private ProductsFilterModel()
        {
            this.RegionIds = new int[0];
            this.DepartmentIds = new int[0];
        }

        [NotNull]
        public int[] RegionIds { get; private set; }
        
        [NotNull]
        public int[] DepartmentIds { get; private set; }

        public static ProductsFilterModel CreateDefault()
        {
            return new ProductsFilterModel();
        }
    }
}