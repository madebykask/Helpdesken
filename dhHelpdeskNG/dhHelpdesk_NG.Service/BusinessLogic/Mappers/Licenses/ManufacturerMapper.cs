namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Domain;

    public static class ManufacturerMapper
    {
        public static ManufacturerOverview[] MapToOverviews(this IQueryable<Manufacturer> query)
        {
            var overviews = query.Select(m => new ManufacturerOverview
                                                  {
                                                      ManufacturerId = m.Id,
                                                      ManufcturerName = m.Name
                                                  }).ToArray();

            return overviews;
        }
    }
}