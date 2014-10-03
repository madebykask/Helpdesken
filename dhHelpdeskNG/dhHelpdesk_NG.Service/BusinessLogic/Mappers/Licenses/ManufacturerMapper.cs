namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Manufacturers;
    using DH.Helpdesk.Domain;

    public static class ManufacturerMapper
    {
        public static ManufacturerOverview[] MapToOverviews(this IQueryable<Manufacturer> query)
        {
            var entities = query.Select(m => new 
                                                {
                                                    ManufacturerId = m.Id,
                                                    ManufcturerName = m.Name
                                                }).ToArray();

            var overviews = entities.Select(m => new ManufacturerOverview(
                                                    m.ManufacturerId,
                                                    m.ManufcturerName)).ToArray();

            return overviews;
        }
    }
}