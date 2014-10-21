namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Common.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Web.Models.Changes;

    internal class ModulesInfoFactory : IModulesInfoFactory
    {
        public CustomerChangesModel GetCustomerChangesModel(CustomerChanges[] customerChanges)
        {
            return new CustomerChangesModel(customerChanges);
        }

        public MyCasesModel GetMyCasesModel(MyCase[] cases)
        {
            return new MyCasesModel(cases);
        }
    }
}