using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DH.Helpdesk.Web.Models.Contract
{    

    public class ContractIndexViewModel
    {

        public ContractIndexViewModel(Customer customer)
        {
            Customer = customer;
            Rows = new ContractsIndexRowsModel(customer);
        }

        public Customer Customer { get; private set; }
        public List<ContractCategory> ContractCategories { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public ContractsIndexRowsModel Rows { get; set; }
    }

    public sealed class ContractsIndexRowModel
    {

        public ContractsIndexRowModel()
        {

        }
        public int ContractId { get; set; }

        public string ContractNumber { get; set; }

        public DateTime? ContractStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public int Finished { get; set; }

        public int Running { get; set; }

        public int FollowUpInterval { get; set; }

        public string Info { get; set; }

        public DateTime? NoticeDate { get; set; }

        public ContractCategory ContractCategory { get; set; }

        public Supplier Supplier { get; set; }

        public Department Department { get; set; }

        public User ResponsibleUser { get; set; }

        public User FollowUpResponsibleUser { get; set; }
    }

    public sealed class ContractsIndexRowsModel
    {
        public ContractsIndexRowsModel(Customer customer)
        {
            Data = new List<ContractsIndexRowModel>();
            Customer = customer;
        }
        public Customer Customer { get; private set; }
        public List<ContractsIndexRowModel> Data { get; set; }
    }
}