namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Case
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Infrastructure.Cases;
    using DH.Helpdesk.Mobile.Models.Customers;

    public interface ICaseModelFactory
    {
        CustomersInfoViewModel CreateCustomersInfoModel(
                                            ICasesCalculator calculator,
                                            IList<Case> cases,
                                            IList<CustomerUser> customers,
                                            int userId);
    }
}