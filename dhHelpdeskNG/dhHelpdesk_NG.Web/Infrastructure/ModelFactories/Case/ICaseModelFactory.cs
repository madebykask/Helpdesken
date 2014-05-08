namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Case
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Infrastructure.Cases;
    using DH.Helpdesk.Web.Models.Customers;

    public interface ICaseModelFactory
    {
        CustomersInfoViewModel CreateCustomersInfoModel(
                                            ICasesCalculator calculator,
                                            IList<Case> cases,
                                            IList<CustomerUser> customers,
                                            int userId);
    }
}