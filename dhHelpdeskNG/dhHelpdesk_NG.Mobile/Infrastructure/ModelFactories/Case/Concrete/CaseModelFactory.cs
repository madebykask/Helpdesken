namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Case.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Infrastructure.Cases;
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
    }
}