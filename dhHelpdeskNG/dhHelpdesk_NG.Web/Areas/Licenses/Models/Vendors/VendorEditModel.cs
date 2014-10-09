namespace DH.Helpdesk.Web.Areas.Licenses.Models.Vendors
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class VendorEditModel
    {
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        [LocalizedDisplay("Försäljare")]
        public string VendorName { get; set; }

        [LocalizedDisplay("Kontakt")]
        [LocalizedStringLength(50)]
        public string Contact { get; set; }

        [LocalizedStringLength(50)]
        [LocalizedDisplay("adress")]
        public string Address { get; set; }

        [LocalizedStringLength(10)]
        [LocalizedDisplay("Postnummer")]
        public string PostalCode { get; set; }

        [LocalizedStringLength(50)]
        [LocalizedDisplay("Postadress")]
        public string PostalAddress { get; set; }

        [LocalizedStringLength(50)]
        [LocalizedDisplay("Telefon")]
        public string Phone { get; set; }

        [LocalizedStringLength(50)]
        [LocalizedDisplay("E-post")]
        public string Email { get; set; }

        [LocalizedStringLength(50)]
        [LocalizedDisplay("Hemsida")]
        public string HomePage { get; set; }
    }
}