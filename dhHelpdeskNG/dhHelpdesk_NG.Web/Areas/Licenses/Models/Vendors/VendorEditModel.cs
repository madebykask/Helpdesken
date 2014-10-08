namespace DH.Helpdesk.Web.Areas.Licenses.Models.Vendors
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class VendorEditModel
    {
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        [LocalizedDisplay("Försäljare")]
        public string VendorName { get; set; }

        [LocalizedRequired]
        [LocalizedDisplay("Kontakt")]
        [LocalizedStringLength(50)]
        public string Contact { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        [LocalizedDisplay("adress")]
        public string Address { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(10)]
        [LocalizedDisplay("Postnummer")]
        public string PostalCode { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        [LocalizedDisplay("Postadress")]
        public string PostalAddress { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        [LocalizedDisplay("Telefon")]
        public string Phone { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        [LocalizedDisplay("E-post")]
        public string Email { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        [LocalizedDisplay("Hemsida")]
        public string HomePage { get; set; }
    }
}