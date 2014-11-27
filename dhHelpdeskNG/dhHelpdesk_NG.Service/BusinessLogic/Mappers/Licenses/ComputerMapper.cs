namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Computers;
    using DH.Helpdesk.Domain.Computers;

    public static class ComputerMapper
    {
        public static ComputerOverview[] MapToOverviews(this IQueryable<Computer> query)
        {
            var entities = query.Select(c => new
                                            {
                                                 ComputerId = c.Id,
                                                 c.ComputerName,
                                                 User = c.User.FirstName + " " + c.User.SurName,
                                                 c.ScanDate
                                            })
                                            .OrderBy(c => c.ComputerName)
                                            .ToArray();

            return entities.Select(c => new ComputerOverview(
                                                c.ComputerId,
                                                c.ComputerName,
                                                c.User,
                                                c.ScanDate)).ToArray();
        }
    }
}