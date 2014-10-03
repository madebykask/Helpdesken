namespace DH.Helpdesk.BusinessData.Models.Licenses.Products
{
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProductsFilterData
    {
        public ProductsFilterData(
                ItemOverview[] regions, 
                ItemOverview[] departments)
        {
            this.Departments = departments;
            this.Regions = regions;
        }

        [NotNull]
        public ItemOverview[] Regions { get; private set; }

        [NotNull]
        public ItemOverview[] Departments { get; private set; }
    }
}