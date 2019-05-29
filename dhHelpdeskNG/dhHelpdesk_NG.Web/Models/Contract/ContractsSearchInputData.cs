using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Web.Models.Contract
{
    public class ContractsSearchInputData
    {
        public ContractsSearchInputData()
        {
            Categories = new List<int>();
            Suppliers = new List<int>();
            ResponsibleUsers = new List<int>();
            ResponsibleFollowUpUsers = new List<int>();
            Departments = new List<int>();
        }

        public int CustomerId { get; set; }

        public List<int> Categories { get; set; }

        public List<int> Suppliers { get; set; }

        public List<int> ResponsibleUsers { get; set; }
        public List<int> ResponsibleFollowUpUsers { get; set; }


        public List<int> Departments { get; set; }

        public int ShowContracts { get; set; }

        public DateTime? StartDateTo { get; set; }
        public DateTime? StartDateFrom { get; set; }
        
        public DateTime? EndDateTo { get; set; }
        public DateTime? EndDateFrom { get; set; }
        
        public DateTime? NoticeDateTo { get; set; }
        public DateTime? NoticeDateFrom { get; set; }

        public string SearchText { get; set; }

        public string SortColName { get; set; }

        public bool SortAsc { get; set; }
    }
}