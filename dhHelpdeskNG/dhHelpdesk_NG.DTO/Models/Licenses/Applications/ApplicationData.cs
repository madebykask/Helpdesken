namespace DH.Helpdesk.BusinessData.Models.Licenses.Applications
{
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ApplicationData
    {
        public ApplicationData(
                ApplicationModel application, 
                ItemOverview[] products)
        {
            this.Products = products;
            this.Application = application;
        }

        [NotNull]
        public ApplicationModel Application { get; private set; }

        [NotNull]
        public ItemOverview[] Products { get; private set; }
    }
}