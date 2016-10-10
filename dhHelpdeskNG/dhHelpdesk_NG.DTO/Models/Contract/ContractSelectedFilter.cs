namespace DH.Helpdesk.BusinessData.Models.Contract
{
    using DH.Helpdesk.BusinessData.Models.Shared;
    using System.Collections.Generic;

    public class ContractSelectedFilter
    {
        public ContractSelectedFilter()
        {
            this.SelectedContractCategories = new SelectedItems();
            this.SelectedSuppliers = new SelectedItems();           
        }

        public int CustomerId { get; set; }

        public SelectedItems SelectedContractCategories { get; set; }

        public SelectedItems SelectedSuppliers { get; set; }

    }
}