namespace DH.Helpdesk.Web.Areas.Licenses.Models.Products
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class ProductEditModel
    {
        public ProductEditModel(
                int id,
                int customerId,
                string productName,
                MultiSelectList applications)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.Applications = applications;
            this.ProductName = productName;
        }

        public ProductEditModel()
        {            
        }

        [IsId]
        [HiddenInput]
        public int Id { get; set; }

        [IsId]
        [HiddenInput]
        public int CustomerId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        [LocalizedDisplay("Produkt")]
        public string ProductName { get; set; }

        [NotNull]
        public MultiSelectList Applications { get; private set; }

        [NotNull]
        public int[] ApplicationIds { get; set; }
    }
}