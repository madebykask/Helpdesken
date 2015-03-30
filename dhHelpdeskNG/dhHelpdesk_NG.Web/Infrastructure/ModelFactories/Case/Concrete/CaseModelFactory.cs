namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Case.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
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

        public CaseRemainingTimeViewModel GetCaseRemainingTimeModel(CaseRemainingTimeData data)
        {
            return new CaseRemainingTimeViewModel(data);
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
    }
}