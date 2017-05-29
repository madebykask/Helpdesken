using System.Collections.Generic;

namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.ComponentModel.DataAnnotations;


    public interface IContractSearch : ISearch
    {
        int CustomerId { get; set; }
        List<int> CategoryId { get; set; }

        List<int> SupplierId { get; set; }

        List<int> ResponsibleId { get; set; }

        List<int> DepartmentId { get; set; }

        List<int> ShowId { get; set; }

        string Text_Filter { get; set; }
    }

    public class ContractSearch : Search, IContractSearch
    {
        public int CustomerId { get; set; }

        public List<int> CategoryId { get; set; }

        public List<int> SupplierId { get; set; }

        public List<int> ResponsibleId { get; set; }

        public List<int> DepartmentId { get; set; }

        public List<int> ShowId { get; set; }

        public string Text_Filter { get; set; }



    }
}
