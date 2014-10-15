namespace DH.Helpdesk.Web.Areas.Licenses.Models.Products
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class ProductEditModel : BaseEditModel
    {
        public ProductEditModel(
                int id,
                int customerId,
                string productName,
                MultiSelectListModel applications)
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
        public int CustomerId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        [LocalizedDisplay("Produkt")]
        public string ProductName { get; set; }

        [NotNull]
        public MultiSelectListModel Applications { get; set; }

        public override EntityModelType Type
        {
            get
            {
                return EntityModelType.Products;
            }
        }
    }
}