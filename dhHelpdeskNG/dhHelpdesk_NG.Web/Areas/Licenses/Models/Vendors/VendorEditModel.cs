namespace DH.Helpdesk.Web.Areas.Licenses.Models.Vendors
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class VendorEditModel : BaseEditModel
    {
        public VendorEditModel(
                int id,
                int customerId,
                string vendorName,
                string contact,
                string address,
                string postalCode,
                string postalAddress,
                string phone,
                string email,
                string homePage)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.VendorName = vendorName;
            this.Contact = contact;
            this.Address = address;
            this.PostalCode = postalCode;
            this.PostalAddress = postalAddress;
            this.Phone = phone;
            this.Email = email;
            this.HomePage = homePage;
        }

        public VendorEditModel()
        {            
        }

        [IsId]
        [HiddenInput]
        public int CustomerId { get; set; }

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

        public override string Title
        {
            get
            {
                return "Försäljare";
            }
        }
    }
}