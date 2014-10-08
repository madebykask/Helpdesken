namespace DH.Helpdesk.Web.Areas.Licenses.Models.Licenses
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class LicenseEditModel
    {
        public LicenseEditModel(
                SelectList products, 
                SelectList departments, 
                SelectList vendors)
        {
            this.Vendors = vendors;
            this.Departments = departments;
            this.Products = products;
        }

        [NotNull]
        public SelectList Products { get; private set; }

        [IsId]
        [LocalizedDisplay("Produkt")]
        public int? ProductId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(100)]
        [LocalizedDisplay("Licensnummer")]
        public string LicenseNumber { get; set; }

        [LocalizedRequired]
        [LocalizedInteger]
        [LocalizedMin(1)]
        [LocalizedDisplay("Antal licenser")]
        public int NumberOfLicenses { get; set; }

        [NotNull]
        public SelectList Departments { get; private set; }

        [IsId]
        [LocalizedDisplay("Avdelning")]
        public int DepartmentId { get; set; }

        [LocalizedDisplay("Inköpsdatum")]
        public DateTime? PurchaseDate { get; set; }

        [LocalizedInteger]
        [LocalizedDisplay("Inköpspris")]
        public int Price { get; set; }

        [LocalizedStringLength(200)]
        [LocalizedDisplay("Inköpsunderlag")]
        public string PurchaseInfo { get; set; }

        [LocalizedDisplay("Giltig t.o.m")]
        public DateTime? ValidDate { get; set; }

        [NotNull]
        public SelectList Vendors { get; private set; }

        [LocalizedRequired]
        [LocalizedInteger]
        [LocalizedDisplay("Årlig kostnad")]
        public int PriceYear { get; set; }

        [LocalizedDisplay("Kommentar")]
        public string Info { get; set; }
    }
}