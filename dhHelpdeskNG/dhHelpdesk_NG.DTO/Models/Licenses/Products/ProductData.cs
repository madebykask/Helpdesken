namespace DH.Helpdesk.BusinessData.Models.Licenses.Products
{
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProductData
    {
        public ProductData(
                ProductModel product, 
                ItemOverview[] manufacturers,
                ItemOverview[] availableApplications,
                ItemOverview[] applications)
        {
            this.AvailableApplications = availableApplications;
            this.Applications = applications;
            this.Product = product;
            this.Manufacturers = manufacturers;
        }

        [NotNull]
        public ProductModel Product { get; private set; }

        public ItemOverview[] Manufacturers { get; private set; }

        [NotNull]
        public ItemOverview[] AvailableApplications { get; private set; }

        [NotNull]
        public ItemOverview[] Applications { get; private set; }
    }
}