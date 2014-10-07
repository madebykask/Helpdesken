namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Applications;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;

    public static class ApplicationMapper
    {
        public static ApplicationOverview[] MapToOverviews(this IQueryable<Application> query, IQueryable<Product> products)
        {
            var entities = query.SelectMany(
                                        a => products,
                                        (a, p) => new 
                                                {
                                                    ApplicationId = a.Id,
                                                    ApplicationName = a.Name,
                                                    InstallationsNumber = a.Products.Count(),
                                                    ProductName = p.Name
                                                  }).ToArray();

            var overviews = entities.Select(a => new ApplicationOverview(
                                                    a.ApplicationId,
                                                    a.ApplicationName,
                                                    a.ProductName,
                                                    a.InstallationsNumber)).ToArray();

            return overviews;
        }

        public static ApplicationModel MapToBusinessModel(this IQueryable<Application> query, int id)
        {
            ApplicationModel model = null;

            var entity = query.GetById(id)
                        .Select(a => new
                                {
                                    a.Customer_Id,
                                    a.Name,
                                    a.CreatedDate,
                                    a.ChangedDate
                                }).SingleOrDefault();

            if (entity != null)
            {
                model = new ApplicationModel(
                                    entity.Customer_Id,
                                    entity.Name,
                                    entity.CreatedDate,
                                    entity.ChangedDate);
            }

            return model;
        }

        public static void MapToEntity(ApplicationModel model, Application entity)
        {
            entity.Customer_Id = model.CustomerId;
            entity.Name = model.ApplicationName;
        }
    }
}