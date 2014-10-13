namespace DH.Helpdesk.Web.Areas.Licenses.Models.Applications
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class ApplicationEditModel : BaseEditModel
    {
        public ApplicationEditModel(
                int id,
                int customerId,
                string applicationName,
                SelectList products)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.ApplicationName = applicationName;
            this.Products = products;
        }

        public ApplicationEditModel()
        {            
        }

        [IsId]
        [HiddenInput]
        public int CustomerId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(100)]
        [LocalizedDisplay("Applikation")]
        public string ApplicationName { get; set; }

        [NotNull]
        public SelectList Products { get; private set; }

        [IsId]
        [LocalizedDisplay("Produkt")]
        public int? ProductId { get; set; }

        public override EntityModelType Type
        {
            get
            {
                return EntityModelType.Applications;
            }
        }
    }
}