namespace DH.Helpdesk.Web.Models.Case
{
    using DH.Helpdesk.BusinessData.Models.Case.Output;

    public sealed class CustomerCasesModel
    {
        public CustomerCasesModel(CustomerCases[] customerCases)
        {
            this.CustomerCases = customerCases;
        }

        public CustomerCasesModel()
        {
            this.CustomerCases = new CustomerCases[0];
        }

        public CustomerCases[] CustomerCases { get; private set; }
    }
}