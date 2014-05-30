namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Translate;
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

        ItemOverview GetOverview(int customerId, ReportType reportType);
    }

    public class ReportCustomerRepository : RepositoryBase<ReportCustomer>, IReportCustomerRepository
    {
        private readonly ITranslator translator;

        public ReportCustomerRepository(IDatabaseFactory databaseFactory, ITranslator translator)
            : base(databaseFactory)
        {
            this.translator = translator;
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
            var entities = this.Table
                    .Where(r => r.Customer_Id == customerId && 
                                r.ShowOnPage == 1)
                    .Select(r => new
                            {
                                ReportId = r.Report_Id,
                            })
                    .ToList();

            return entities
                .Select(r => new ItemOverview(this.GetReportName((ReportType)r.ReportId), r.ReportId.ToString(global::System.Globalization.CultureInfo.InvariantCulture)))
                .Where(i => !string.IsNullOrEmpty(i.Name))
                .OrderBy(i => i.Name);
        }

        public ItemOverview GetOverview(int customerId, ReportType reportType)
        {
            var entities = this.Table
                    .Where(r => r.Customer_Id == customerId &&
                                r.Report_Id == (int)reportType &&
                                r.ShowOnPage == 1)
                    .Select(r => new
                    {
                        ReportId = r.Report_Id,
                    })
                    .ToList();

            return entities
                .Select(r => new ItemOverview(this.GetReportName((ReportType)r.ReportId), r.ReportId.ToString(global::System.Globalization.CultureInfo.InvariantCulture)))
                .FirstOrDefault();            
        }

        private string GetReportName(ReportType reportType)
        {
            switch (reportType)
            {
                case ReportType.LeadtimeFinishedCases:
                    return this.translator.Translate("Rapport - Ledtid (avslutade ärenden)");
                case ReportType.LeadtimeActiveCases:
                    return this.translator.Translate("Rapport - Ledtid (aktiva ärenden)");
                case ReportType.FinishingCauseCustomer:
                    return string.Format(
                        "{0} - {1}", 
                        this.translator.Translate("Rapport"),
                        this.translator.Translate("Avslutsorsak"));
                case ReportType.FinishingCauseCategoryCustomer:
                    return string.Format(
                        "{0} - {1}", 
                        this.translator.Translate("Rapport"),
                        this.translator.Translate("Avslutskategori"));
                case ReportType.ClosedCasesDay:
                    return string.Format(
                        "{0} - {1}/{2}", 
                        this.translator.Translate("Rapport"),
                        this.translator.Translate("Avslutade ärenden"),
                        this.translator.Translate("dag"));
                case ReportType.RegistratedCasesDay:
                    return string.Format(
                        "{0} - {1}/{2}", 
                        this.translator.Translate("Rapport"),
                        this.translator.Translate("Registrerade ärenden"),
                        this.translator.Translate("dag"));
                case ReportType.RegistratedCasesHour:
                    return string.Format(
                        "{0} - {1}/{2}", 
                        this.translator.Translate("Rapport"),
                        this.translator.Translate("Registrerade ärenden"),
                        this.translator.Translate("timme"));
                case ReportType.CasesInProgressDay:
                    return string.Format(
                        "{0} - {1}/{2}", 
                        this.translator.Translate("Rapport"),
                        this.translator.Translate("Pågående ärenden"),
                        this.translator.Translate("dag"));
                case ReportType.ServiceReport:
                    return this.translator.Translate("Rapport - Servicerapport");
                case ReportType.RegistratedCasesCaseType:
                    return string.Format(
                        "{0} - {1}/{2}", 
                        this.translator.Translate("Rapport"),
                        this.translator.Translate("Registrerade ärenden"),
                        this.translator.Translate("Case Type"));
                case ReportType.CaseTypeArticleNo:
                    return string.Format(
                        "{0} - {1}", 
                        this.translator.Translate("Rapport"),
                        this.translator.Translate("Case Type/Article No"));
                case ReportType.AverageSolutionTime:
                    return string.Format(
                        "{0} - {1}", 
                        this.translator.Translate("Rapport"),
                        this.translator.Translate("Genomsnittlig lösningstid"));
                case ReportType.RegistrationSource:
                    return string.Format(
                        "{0} - {1}", 
                        this.translator.Translate("Rapport"),
                        this.translator.Translate("Källa registrering"));
                case ReportType.ResponseTime:
                    return string.Format(
                        "{0} - {1}", 
                        this.translator.Translate("Rapport"),
                        this.translator.Translate("Svarstid"));
                case ReportType.ReportGenerator:
                    return string.Format(
                        "{0} - {1}", 
                        this.translator.Translate("Rapport"),
                        this.translator.Translate("Rapportgenerator"));
            }

            return null;
        }
    }

    #endregion
}
