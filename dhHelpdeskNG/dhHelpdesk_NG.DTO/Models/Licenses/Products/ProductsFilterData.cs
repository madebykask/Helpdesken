namespace DH.Helpdesk.BusinessData.Models.Licenses.Products
{
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProductsFilterData
    {
        public ProductsFilterData(
                ItemOverview[] regions, 
                ItemOverview[] departments,
                ItemOverview[] products)
        {
            this.Departments = departments;
            this.Regions = regions;
            this.Products = products;
        }

        [NotNull]
        public ItemOverview[] Regions { get; private set; }

        [NotNull]
        public ItemOverview[] Departments { get; private set; }

        [NotNull]
        public ItemOverview[] Products { get; private set; }
    }
}