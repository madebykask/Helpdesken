namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public class ChangeObjectRepository : RepositoryBase<ChangeObject>, IChangeObjectRepository
    {
        public ChangeObjectRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverviewDto> FindOverviewsByCustomerId(int customerId)
        {
            var objects = this.DataContext.ChangeObjects.Where(o => o.Customer_Id == customerId);
            var overviews = objects.Select(o => new { Name = o.Name, Value = o.Id }).ToList();
            
            return
                overviews.Select(
                    o => new ItemOverviewDto { Name = o.Name, Value = o.Value.ToString(CultureInfo.InvariantCulture) })
                         .ToList();
        }
    }
}