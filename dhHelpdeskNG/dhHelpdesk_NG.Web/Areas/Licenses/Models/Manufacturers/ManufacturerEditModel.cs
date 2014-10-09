namespace DH.Helpdesk.Web.Areas.Licenses.Models.Manufacturers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class ManufacturerEditModel
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
        public int Id { get; set; }

        [IsId]
        [HiddenInput]
        public int CustomerId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(100)]
        [LocalizedDisplay("Tillverkare")]
        public string ManufcturerName { get; set; }
    }
}