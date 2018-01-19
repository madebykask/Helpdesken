namespace DH.Helpdesk.Services.Services.Concrete
{
    using Dal.Repositories;
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Dal.Repositories.Invoice;
    using System.Linq;

    public sealed class CaseInvoiceSettingsService : ICaseInvoiceSettingsService
    {
        private readonly ICaseInvoiceSettingsRepository caseInvoiceSettingsRepository;
        private readonly IDepartmentRepository departmentRepository;


        public CaseInvoiceSettingsService(
            ICaseInvoiceSettingsRepository caseInvoiceSettingsRepository,
            IDepartmentRepository departmentRepository)
        {
            this.caseInvoiceSettingsRepository = caseInvoiceSettingsRepository;
            this.departmentRepository = departmentRepository;
        }

        public CaseInvoiceSettings GetSettings(int customerId)
        {
            return this.caseInvoiceSettingsRepository.GetSettings(customerId);
        }

        public int SaveSettings(CaseInvoiceSettings settings)
        {
            var ret = this.caseInvoiceSettingsRepository.SaveSettings(settings);

            var disabledDepsForSendInvocie = departmentRepository.GetMany(d => d.Customer_Id == settings.CustomerId && d.DisabledForOrder)
                                                                 .ToList();

            if (settings.DisabledDepartmentIds == null)
                settings.DisabledDepartmentIds = new int[] { };

            var depsToEnable = disabledDepsForSendInvocie.Where(d => !settings.DisabledDepartmentIds.Contains(d.Id))
                                                         .Select(d=> d.Id).ToArray();

            departmentRepository.UpdateDeparmentDisabledForOrder(depsToEnable, settings.DisabledDepartmentIds);
            departmentRepository.Commit();

            return ret;   
        }
    }
}