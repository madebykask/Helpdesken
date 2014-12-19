namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Survey
{
    using System;
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class SurveySepecifications
    {
        public static IQueryable<Survey> WhereCases(this IQueryable<Survey> query, IQueryable<int> caseIds)
        {
            if (caseIds != null)
            {
                query = query.Where(c => caseIds.Contains(c.CaseId));
            }

            return query;
        }
    }
}
