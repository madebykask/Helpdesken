namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Common.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Mobile.Models.Case;
    using DH.Helpdesk.Mobile.Models.Changes;

    internal class ModulesInfoFactory : IModulesInfoFactory
    {
        public CustomerChangesModel GetCustomerChangesModel(
                IList<CustomerUser> customers,
                IEnumerable<CustomerChange> changes,
                int userId)
        {
            return new CustomerChangesModel(customers, changes, userId);
        }

        public MyCasesModel GetMyCasesModel(MyCase[] cases)
        {
            return new MyCasesModel(cases);
        }
    }
}