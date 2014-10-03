namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Vendors;
    using DH.Helpdesk.Domain;

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
    }
}