using System;


namespace DH.Helpdesk.BusinessData.Models.Contract
{
    using System.Collections.Generic;

    public class ContractsSearchFilter
    {
        public ContractsSearchFilter(int customerId)
        {
            this.CustomerId = customerId;
            this.SelectedContractCategories = new List<int>();
            this.SelectedSuppliers = new List<int>();
            this.SelectedResponsibles = new List<int>();
            SelectedResponsibleFollowUpUsers = new List<int>();
            this.SelectedDepartments = new List<int>();
            this.State = 0; 
            this.SearchText = string.Empty;
        }

        public int CustomerId { get; private set; }

        public List<int> SelectedContractCategories { get; set; }

        public List<int> SelectedSuppliers { get; set; }

        public List<int> SelectedResponsibles { get; set; }
        public List<int> SelectedResponsibleFollowUpUsers { get; set; }
        public List<int> SelectedDepartments { get; set; }

        public int State { get; set; }

        public DateTime? StartDateTo { get; set; }
        public DateTime? StartDateFrom { get; set; }

        public DateTime? EndDateTo { get; set; }
        public DateTime? EndDateFrom { get; set; }

        public DateTime? NoticeDateTo { get; set; }
        public DateTime? NoticeDateFrom { get; set; }

        public string SearchText { get; set; }
    }
}