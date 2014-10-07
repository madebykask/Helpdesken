namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Vendors;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;

    public static class VendorMapper
    {
        public static VendorOverview[] MapToOverviews(this IQueryable<Vendor> query)
        {
            var entities = query.Select(v => new 
                                                {
                                                    VendorId = v.Id,
                                                    v.VendorName,
                                                    VendorContact = v.Contact,
                                                    VendorPhone = v.Phone,
                                                    VendorEmail = v.EMail,
                                                    VendorHomePage = v.HomePage
                                                }).ToArray();

            var overviews = entities.Select(v => new VendorOverview(
                                                    v.VendorId,
                                                    v.VendorName,
                                                    v.VendorContact,
                                                    v.VendorPhone,
                                                    v.VendorEmail,
                                                    v.VendorHomePage)).ToArray();

            return overviews;
        }

        public static VendorModel MapToBusinessModel(this IQueryable<Vendor> query, int id)
        {
            VendorModel model = null;

            var entity = query.GetById(id)
                        .Select(v => new
                            {
                                v.VendorName,
                                v.Contact,
                                v.Address,
                                v.PostalCode,
                                v.PostalAddress,
                                v.Phone,
                                v.EMail,
                                v.HomePage,
                                v.Customer_Id,
                                v.CreatedDate,
                                v.ChangedDate
                            }).SingleOrDefault();

            if (entity != null)
            {
                model = new VendorModel(
                                entity.VendorName,
                                entity.Contact,
                                entity.Address,
                                entity.PostalCode,
                                entity.PostalAddress,
                                entity.Phone,
                                entity.EMail,
                                entity.HomePage,
                                entity.Customer_Id,
                                entity.CreatedDate,
                                entity.ChangedDate);
            }

            return model;
        }

        public static void MapToEntity(VendorModel model, Vendor entity)
        {
            entity.VendorName = model.VendorName;
            entity.Contact = model.Contact;
            entity.Address = model.Address;
            entity.PostalCode = model.PostalCode;
            entity.PostalAddress = model.PostalAddress;
            entity.Phone = model.Phone;
            entity.EMail = model.Email;
            entity.HomePage = model.HomePage;
            entity.Customer_Id = model.CustomerId;
        }
    }
}