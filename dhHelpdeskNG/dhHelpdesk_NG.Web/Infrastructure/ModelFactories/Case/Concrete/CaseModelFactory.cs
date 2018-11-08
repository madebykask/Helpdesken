using System;
using DH.Helpdesk.BusinessData.Models.Paging;
using DH.Helpdesk.Web.Common.Models.CaseSearch;
using DH.Helpdesk.Web.Models;

namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Case.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Infrastructure.Cases;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Web.Models.Customers;

    internal sealed class CaseModelFactory : ICaseModelFactory
    {
        public CustomersInfoViewModel CreateCustomersInfoModel(
                                ICasesCalculator calculator, 
                                IList<Case> cases, 
                                IList<CustomerUser> customers,
                                int userId)
        {
            calculator.CollectCases(cases);

            var instance = new CustomersInfoViewModel(
                                calculator,
                                cases,
                                customers,
                                userId);
                               
            return instance;
        }

        public CustomerCasesModel CreateCustomerCases(CustomerCases[] customerCases)
        {
            return new CustomerCasesModel(customerCases);
        }

        public RelatedCasesViewModel GetRelatedCasesModel(List<RelatedCase> relatedCases, int customerId, string userId)
        {
            return new RelatedCasesViewModel(relatedCases, customerId, userId);
        }

        public CaseRemainingTimeViewModel GetCaseRemainingTimeModel(CaseRemainingTimeData data, IWorkContext workContext)
        {
            return new CaseRemainingTimeViewModel(data, workContext);
        }

        public RelatedCasesFullViewModel GetRelatedCasesFullModel(
                                CaseSearchResultModel searchResult, 
                                string userId, 
                                int caseId,
                                string sortBy,
                                bool sortByAsc)
        {
            return new RelatedCasesFullViewModel(searchResult, userId, caseId, sortBy, sortByAsc);
        }

        public CaseSearchModel InitEmptySearchModel(int customerId, int userId)
        {
            ISearch s = new Search();
            var f = new CaseSearchFilter
            {
                CustomerId = customerId,
                UserId = userId,
                CaseType = 0,
                Category = null,
                Priority = null,
                ProductArea = null,
                Region = null,
                StateSecondary = null,
                Status = null,
                User = null,
                UserPerformer = null,
                UserResponsible = null,
                WorkingGroup = null,
                CaseProgress = CaseSearchFilter.InProgressCases,
                CaseRegistrationDateStartFilter = null,
                CaseRegistrationDateEndFilter = null,
                CaseWatchDateStartFilter = null,
                CaseWatchDateEndFilter = null,
                CaseClosingDateStartFilter = null,
                CaseClosingDateEndFilter = null,
                CaseClosingReasonFilter = null,
                PageInfo = new PageInfo()
            };

            s.SortBy = "CaseNumber";
            s.Ascending = true;

            return new CaseSearchModel() { CaseSearchFilter = f, Search = s };
        }
    }
}