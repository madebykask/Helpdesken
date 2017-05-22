namespace DH.Helpdesk.BusinessData.Models.Contract
{
    using DH.Helpdesk.BusinessData.Models.Shared;
    using System.Collections.Generic;
    

    public class ContractSelectedFilter
    {

        public enum Show
        {
            Inactive = 0,
            Active = 1,
            All = 2
        }
        public ContractSelectedFilter()
        {
            this.SelectedContractCategories = new SelectedItems();
            this.SelectedSuppliers = new SelectedItems();
            this.SelectedResponsibles = new SelectedItems();
            this.SelectedDepartments = new SelectedItems();
            this.State = Show.Active;
            this.SearchText = string.Empty;
        }

        public int CustomerId { get; set; }

        public SelectedItems SelectedContractCategories { get; set; }

        public SelectedItems SelectedSuppliers { get; set; }

        public SelectedItems SelectedResponsibles { get; set; }

        public SelectedItems SelectedDepartments { get; set; }

        public Show State { get; set; }

        public string SearchText { get; set; }

    }
}