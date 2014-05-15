namespace DH.Helpdesk.Services.Services.Concrete
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Reports.Output;
    using DH.Helpdesk.Dal.Infrastructure.Translate;
    using DH.Helpdesk.Dal.Repositories;

    public sealed class ReportsService : IReportsService
    {
        private readonly IReportCustomerRepository reportCustomerRepository;

        private readonly ITranslator translator;

        public ReportsService(
            IReportCustomerRepository reportCustomerRepository, 
            ITranslator translator)
        {
            this.reportCustomerRepository = reportCustomerRepository;
            this.translator = translator;
        }

        public SearchData GetSearchData(OperationContext context)
        {
            var reports = this.reportCustomerRepository.FindOverviews(context.CustomerId);
            var options = new SearchOptions(reports);
            var reportsSettings = new FieldOverviewSetting(true, this.translator.Translate("Rapport"));
            var settings = new SearchSettings(reportsSettings);
            return new SearchData(options, settings);
        }
    }
}