namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Manufacturers;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;

    public static class ManufacturerMapper
    {
        public static ManufacturerOverview[] MapToOverviews(this IQueryable<Manufacturer> query)
        {
            var entities = query.Select(m => new 
                                                {
                                                    ManufacturerId = m.Id,
                                                    ManufcturerName = m.Name
                                                })
                                                .OrderBy(m => m.ManufcturerName)
                                                .ToArray();

            var overviews = entities.Select(m => new ManufacturerOverview(
                                                    m.ManufacturerId,
                                                    m.ManufcturerName)).ToArray();

            return overviews;
        }

        public static ManufacturerModel MapToBusinessModel(this IQueryable<Manufacturer> query, int id)
        {
            ManufacturerModel model = null;

            var entity = query.GetById(id)
                        .Select(m => new
                            {
                                m.Id,
                                m.Customer_Id,
                                m.Name,
                                m.CreatedDate,
                                m.ChangedDate
                            }).SingleOrDefault();

            if (entity != null)
            {
                model = new ManufacturerModel(
                                entity.Id,
                                entity.Customer_Id,
                                entity.Name,
                                entity.CreatedDate,
                                entity.ChangedDate);
            }

            return model;
        }

        public static void MapToEntity(ManufacturerModel model, Manufacturer entity)
        {
            entity.Customer_Id = model.CustomerId;
            entity.Name = model.ManufacturerName;
        }
    }
}