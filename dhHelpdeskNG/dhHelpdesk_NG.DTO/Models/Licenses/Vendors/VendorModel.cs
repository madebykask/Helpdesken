namespace DH.Helpdesk.BusinessData.Models.Licenses.Vendors
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class VendorModel : Shared.Input.BusinessModel
    {
        public VendorModel(
                int id,
                string vendorName, 
                string contact, 
                string address, 
                string postalCode, 
                string postalAddress, 
                string phone, 
                string email, 
                string homePage, 
                int customerId, 
                DateTime createdDate, 
                DateTime changedDate)
        {
            this.Id = id;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
            this.CustomerId = customerId;
            this.HomePage = homePage;
            this.Email = email;
            this.Phone = phone;
            this.PostalAddress = postalAddress;
            this.PostalCode = postalCode;
            this.Address = address;
            this.Contact = contact;
            this.VendorName = vendorName;
        }

        public VendorModel(
                int id,
                string vendorName, 
                string contact, 
                string address, 
                string postalCode, 
                string postalAddress, 
                string phone, 
                string email, 
                string homePage, 
                int customerId)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.HomePage = homePage;
            this.Email = email;
            this.Phone = phone;
            this.PostalAddress = postalAddress;
            this.PostalCode = postalCode;
            this.Address = address;
            this.Contact = contact;
            this.VendorName = vendorName;
        }

        public VendorModel(int customerId)
        {
            this.CustomerId = customerId;
        }

        [NotNullAndEmpty]
        [MaxLength(50)]
        public string VendorName { get; private set; }

        [MaxLength(50)]
        public string Contact { get; private set; }

        [MaxLength(50)]
        public string Address { get; private set; }

        [MaxLength(10)]
        public string PostalCode { get; private set; }

        [MaxLength(50)]
        public string PostalAddress { get; private set; }

        [MaxLength(50)]
        public string Phone { get; private set; }

        [MaxLength(50)]
        public string Email { get; private set; }

        [MaxLength(50)]
        public string HomePage { get; private set; }

        [IsId]
        public int CustomerId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public static VendorModel CreateDefault(int customerId)
        {
            return new VendorModel(customerId);
        }
    }
}