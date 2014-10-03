namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Applications;
    using DH.Helpdesk.Domain;

    public static class ApplicationMapper
    {
        public static ApplicationOverview[] MapToOverviews(this IQueryable<Application> query)
        {
            var entities = query.Select(a => new 
                                                {
                                                    ApplicationId = a.Id,
                                                    ApplicationName = a.Name,
                                                    InstallationsNumber = a.Products.Count(),
                                                    ProductName = string.Empty
                                                  }).ToArray();

            var overviews = entities.Select(a => new ApplicationOverview(
                                                    a.ApplicationId,
                                                    a.ApplicationName,
                                                    a.ProductName,
                                                    a.InstallationsNumber)).ToArray();

            return overviews;
        }
    }
}