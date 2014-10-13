namespace DH.Helpdesk.Web.Areas.Licenses.Models.Manufacturers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class ManufacturerEditModel : BaseEditModel
    {
        public ManufacturerEditModel(
                int id,
                int customerId,
                string manufcturerName)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.ManufcturerName = manufcturerName;
        }

        public ManufacturerEditModel()
        {            
        }

        [IsId]
        [HiddenInput]
        public int CustomerId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(100)]
        [LocalizedDisplay("Tillverkare")]
        public string ManufcturerName { get; set; }

        public override EntityModelType Type
        {
            get
            {
                return EntityModelType.Manufacturers;
            }
        }
    }
}