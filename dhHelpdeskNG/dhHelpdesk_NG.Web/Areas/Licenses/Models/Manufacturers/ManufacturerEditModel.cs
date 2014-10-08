namespace DH.Helpdesk.Web.Areas.Licenses.Models.Manufacturers
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class ManufacturerEditModel
    {
        [LocalizedRequired]
        [LocalizedStringLength(100)]
        [LocalizedDisplay("Tillverkare")]
        public string ManufcturerName { get; set; }
    }
}