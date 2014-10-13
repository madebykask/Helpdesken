namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Common
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Mobile.Models.Case;
    using DH.Helpdesk.Mobile.Models.Changes;

    public interface IModulesInfoFactory
    {
        CustomerChangesModel GetCustomerChangesModel(
                IList<CustomerUser> customers,
                IEnumerable<CustomerChange> changes,
                int userId);

        MyCasesModel GetMyCasesModel(MyCase[] cases);
    }
}