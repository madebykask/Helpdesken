namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Cases
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Domain;

    public static class CasesMapper
    {
        public static List<RelatedCase> MapToRelatedCases(this IQueryable<Case> query)
        {
            var entities = query.Select(c => new
                                        {
                                             c.Id,
                                             c.CaseNumber,
                                             c.RegTime,
                                             Status = c.Status.Name,
                                             c.Caption,
                                             c.Description
                                        })
                                .OrderByDescending(c => c.RegTime)
                                .ToList();

            return entities.Select(
                            c => new RelatedCase(
                                            c.Id, 
                                            c.CaseNumber, 
                                            c.RegTime, 
                                            c.Status, 
                                            c.Caption, 
                                            c.Description)).ToList();
        } 
    }
}