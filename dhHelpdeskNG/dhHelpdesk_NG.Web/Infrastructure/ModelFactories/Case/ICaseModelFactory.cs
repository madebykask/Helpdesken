using DH.Helpdesk.Web.Models;

namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Case
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Infrastructure.Cases;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Web.Models.Customers;

    public interface ICaseModelFactory
    {
        CustomersInfoViewModel CreateCustomersInfoModel(
                                            ICasesCalculator calculator,
                                            IList<Case> cases,
                                            IList<CustomerUser> customers,
                                            int userId);

        CustomerCasesModel CreateCustomerCases(CustomerCases[] customerCases);

        RelatedCasesViewModel GetRelatedCasesModel(List<RelatedCase> relatedCases, int customerId, string userId);

        CaseRemainingTimeViewModel GetCaseRemainingTimeModel(CaseRemainingTimeData data, IWorkContext workContext);

        RelatedCasesFullViewModel GetRelatedCasesFullModel(
                                    CaseSearchResultModel searchResult, 
                                    string userId, 
                                    int caseId,
                                    string sortBy,
                                    bool sortByAsc);

        CaseSearchModel InitEmptySearchModel(int customerId, int userId);
    }
}