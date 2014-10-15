namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Department
{
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;

    public static class DepartmentMapper
    {
        public static ItemOverview[] MapToItemOverviews(this IQueryable<Domain.Department> query)
        {
            var entities = query.Select(d => new
                                    {
                                        d.Id,
                                        d.DepartmentName
                                    })
                                    .OrderBy(d => d.DepartmentName)
                                    .ToArray();

            var overviews = entities.Select(p => new ItemOverview(
                                            p.DepartmentName,
                                            p.Id.ToString(CultureInfo.InvariantCulture))).ToArray();

            return overviews;
        }         
    }
}