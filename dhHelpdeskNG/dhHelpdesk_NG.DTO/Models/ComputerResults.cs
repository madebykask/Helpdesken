namespace DH.Helpdesk.BusinessData.Models
{
    using System;

    public class ComputerResults
    {        
        public int Id { get; set; }

        public string ComputerName { get; set; }

        public string ComputerType { get; set; }

        public string ComputerTypeDescription { get; set; }

        public string Location { get; set; }

        public DateTime ContractEndDate { get; set; }        
    }
}
