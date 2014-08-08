namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Common
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Models.Changes;

    public interface IModulesInfoFactory
    {
         CustomerChangesModel GetCustomerChangesModel(
                IList<CustomerUser> customers,
                IEnumerable<CustomerChange> changes);
    }
}