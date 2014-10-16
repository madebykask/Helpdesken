namespace DH.Helpdesk.Web.Areas.Licenses.Models.Products
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class ProductEditModel : BaseEditModel
    {
        public ProductEditModel(
                int id,
                int customerId,
                string productName,
                IList<SelectListItem> availableApplications,
                IList<SelectListItem> selectedApplications)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.ProductName = productName;
            this.AvailableApplications = availableApplications;
            this.SelectedApplications = selectedApplications;
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
        public IList<SelectListItem> AvailableApplications { get; set; }

        [NotNull]
        public IList<SelectListItem> SelectedApplications { get; set; }

        public override EntityModelType Type
        {
            get
            {
                return EntityModelType.Products;
            }
        }
    }
}