namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Domain;

    public static class VendorMapper
    {
        public static VendorOverview[] MapToOverviews(this IQueryable<Vendor> query)
        {
            var overviews = query.Select(v => new VendorOverview
                                                  {
                                                      VendorId = v.Id,
                                                      VendorName = v.VendorName,
                                                      VendorContact = v.Contact,
                                                      VendorPhone = v.Phone,
                                                      VendorEmail = v.EMail,
                                                      VendorHomePage = v.HomePage
                                                  }).ToArray();

            return overviews;
        }
    }
}