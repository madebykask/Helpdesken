namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Problems
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.Domain.Problems;

    public static class ProblemMapper
    {
        public static ProblemInfoOverview[] MapToOverviews(this IQueryable<Problem> query)
        {
            var entities = query.Select(p => new
                                        {
                                            p.CreatedDate,
                                            p.ProblemNumber,
                                            p.Name,
                                            p.Customer
                                        }).ToArray();

            return entities.Select(p => new ProblemInfoOverview
                                        {
                                            CreatedDate = p.CreatedDate,
                                            Name = p.Name,
                                            ProblemNumber = p.ProblemNumber,
                                            CustomerName = p.Customer.Name
                                        }).ToArray();
        }
    }
}