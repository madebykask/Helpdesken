namespace DH.Helpdesk.Services.BusinessLogic.Mappers.EmailGroup
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Domain;

    public static class EmailGroupMapper
    {
        public static List<IdAndNameOverview> MapToActiveIdAndNameOverviews(this IQueryable<EmailGroupEntity> query)
        {
            var entities = query.Select(g => new { g.Id, g.Name }).ToList();

            return entities.Select(g => new IdAndNameOverview(g.Id, g.Name)).ToList();
        }
    }
}