namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Common
{
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Web.Models.Changes;

    public interface IModulesInfoFactory
    {
        CustomerChangesModel GetCustomerChangesModel(CustomerChanges[] customerChanges, bool showIcon);

        MyCasesModel GetMyCasesModel(MyCase[] cases);
    }
}