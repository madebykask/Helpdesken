namespace DH.Helpdesk.Mobile.Models.Customers
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Infrastructure.Cases;

    public sealed class CustomersInfoViewModel
    {
        public CustomersInfoViewModel(
                ICasesCalculator calculator, 
                IList<Case> cases, 
                IList<CustomerUser> customers, 
                int userId)
        {
            this.UserId = userId;
            this.Customers = customers;
            this.Cases = cases;
            this.Calculator = calculator;
        }

        public CustomersInfoViewModel()
        {
        }

        [NotNull]
        public ICasesCalculator Calculator { get; private set; }

        [NotNull]
        public IList<Case> Cases { get; private set; }

        [NotNull]
        public IList<CustomerUser> Customers { get; private set; }

        [IsId]
        public int UserId { get; private set; }
    }
}