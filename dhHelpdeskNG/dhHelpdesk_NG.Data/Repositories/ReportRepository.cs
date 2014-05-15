namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region REPORT

    public interface IReportRepository : IRepository<Report>
    {
    }

    public class ReportRepository : RepositoryBase<Report>, IReportRepository
    {
        public ReportRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region REPORTCUSTOMER

    public interface IReportCustomerRepository : IRepository<ReportCustomer>
    {
        IEnumerable<CustomerReportList> GetCustomerReportListForCustomer(int id);

        IEnumerable<ItemOverview> FindOverviews(int customerId);
    }

    public class ReportCustomerRepository : RepositoryBase<ReportCustomer>, IReportCustomerRepository
    {
        public ReportCustomerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<CustomerReportList> GetCustomerReportListForCustomer(int id)
        {
            var query = from rc in this.DataContext.ReportCustomers
                        join c in this.DataContext.Customers on rc.Customer_Id equals c.Id
                        where c.Id == id
                        group rc by new { c.Id, rc.Report_Id, rc.ShowOnPage } into g
                        select new CustomerReportList
                        {
                            CustomerId = g.Key.Id,
                            ReportId = g.Key.Report_Id,
                            ActiveOnPage = g.Key.ShowOnPage
                        };

            return query;
        }

        public IEnumerable<ItemOverview> FindOverviews(int customerId)
        {
            return null;
        }
    }

    #endregion
}
