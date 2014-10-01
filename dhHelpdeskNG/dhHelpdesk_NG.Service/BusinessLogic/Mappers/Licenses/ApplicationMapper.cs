namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Domain;

    public static class ApplicationMapper
    {
        public static ApplicationOverview[] MapToOverviews(this IQueryable<Application> query)
        {
            var overviews = query.Select(a => new ApplicationOverview
                                                  {
                                                      ApplicationId = a.Id,
                                                      ApplicationName = a.Name,
                                                      InstallationsNumber = a.Products.Count(),

                                                      // TODO Fix it
                                                      ProductName = a.Products.First().Name
                                                  }).ToArray();

            return overviews;
        }
    }
}