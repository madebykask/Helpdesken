using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Data.Repositories
{
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
    }

    #endregion
}
