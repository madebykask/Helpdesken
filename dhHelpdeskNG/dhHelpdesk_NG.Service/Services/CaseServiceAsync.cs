using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Dal.MapperData.CaseHistory;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services
{
    public partial class CaseService
    {
        public Task<CustomerCasesStatus> GetCustomerCasesStatusAsync(int customerId, int userId)
        {
            var today = DateTime.Today;
            var customer = _customerRepository.GetOverview(customerId);
            bool caseResponsibleUserIsVisible;
            using (var uow = _unitOfWorkFactory.Create())
            {
                var caseFieldSettingsRep = uow.GetRepository<CaseFieldSetting>();
                caseResponsibleUserIsVisible = caseFieldSettingsRep.GetAll()
                    .Any(cf => cf.Customer_Id == customerId &&
                               cf.Name == GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString() &&
                               cf.ShowOnStartPage == 1);
            }

            var customerCasesQuery = _caseRepository.GetCustomerCases(customerId).AsQueryable();
            return customerCasesQuery.Take(1).Select(res => new CustomerCasesStatus()
            {
                CustomerId = customerId,
                CustomerName = customer.Name,

                MyCases =
                    customerCasesQuery.Count(c => c.FinishingDate == null && c.Deleted == 0 &&
                                                  (c.Performer_User_Id == userId || 
                                                   (c.CaseResponsibleUser_Id == userId && caseResponsibleUserIsVisible))),

                InProgress =
                    customerCasesQuery.Count(c => c.FinishingDate == null && c.Deleted == 0),

                NewToday =
                    customerCasesQuery.Count(c => c.Deleted == 0 && c.FinishingDate == null && DbFunctions.TruncateTime(c.RegTime) == today),

                ClosedToday =
                    customerCasesQuery.Count(c => c.FinishingDate != null && DbFunctions.TruncateTime(c.FinishingDate) == today),
            }).SingleOrDefaultAsync();
        }

        public Task<Case> GetCaseByIdAsync(int id, bool markCaseAsRead = false)
        {
            return _caseRepository.GetCaseByIdAsync(id, markCaseAsRead);
        }

        public Task<List<CaseHistoryMapperData>> GetCaseHistoriesAsync(int caseId)
        {
            return _caseHistoryRepository.GetCaseHistoriesAsync(caseId);
        }

        public Task<Case> GetDetachedCaseByIdAsync(int id)
        {
            return _caseRepository.GetDetachedCaseQuery(id, true)
                .SingleOrDefaultAsync();
        }
    }
}