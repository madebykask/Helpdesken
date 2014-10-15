namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Shared
{
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain.Interfaces;

    public static class GlobalMappers
    {
        public static ItemOverview[] MapToItemOverviews<T>(this IQueryable<T> query)
            where T : class, INamedEntity
        {
            var entities = query.Select(x => new
            {
                x.Id,
                x.Name
            })
            .OrderBy(x => x.Name)
            .ToArray();

            var overviews = entities.Select(x => new ItemOverview(
                                            x.Name,
                                            x.Id.ToString(CultureInfo.InvariantCulture))).ToArray();

            return overviews;
        }         
    }
}