namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Common.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Models.Changes;

    internal class ModulesInfoFactory : IModulesInfoFactory
    {
        public CustomerChangesModel GetCustomerChangesModel(
                IList<CustomerUser> customers,
                IEnumerable<CustomerChange> changes,
                int userId)
        {
            return new CustomerChangesModel(customers, changes, userId);
        }
    }
}