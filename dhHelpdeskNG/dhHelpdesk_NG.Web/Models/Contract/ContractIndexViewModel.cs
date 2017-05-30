﻿using DH.Helpdesk.Domain;
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
            Columns = new ContractsIndexColumnsModel(customer);

            var selectListItems = new List<SelectListItem>();


            selectListItems.AddRange(GetSelectListItem());

            
            ShowContracts = GetSelectListItem();


        }

        private static IEnumerable<SelectListItem> GetSelectListItem()
        {
            var items = new List<SelectListItem>();

            items.Add(new SelectListItem
            {
                Text = string.Format("{0} {1}", "", "Pågående"),
                Value = "1",
                Selected = false
            });

            items.Add(new SelectListItem
            {
                Text = string.Format("{0} {1}", "", "För uppföljning"),
                Value = "2",
                Selected = false
            });

            items.Add(new SelectListItem
            {
                Text = string.Format("{0} {1}", "", "För uppsägning"),
                Value = "3",
                Selected = false
            });

            items.Add(new SelectListItem
            {
                Text = string.Format("{0} {1}", "", "Löpande"),
                Value = "4",
                Selected = false
            });

            items.Add(new SelectListItem
            {
                Text = string.Format("{0} {1}", "", "Avslutade"),
                Value = "9",
                Selected = false
            });

            items.Add(new SelectListItem
            {
                Text = string.Format("{0} {1}", "", "Alla"),
                Value = "10",
                Selected = true
            });

            return items;
        }
        
        public IEnumerable<SelectListItem> ShowContracts { get; private set; }

        public Customer Customer { get; private set; }
        public List<ContractCategory> ContractCategories { get; set; }
        public IList<User> Users { get; set; }

        public IList<Department> Departments { get; set; }

        public List<Supplier> Suppliers { get; set; }
        public ContractsIndexRowsModel Rows { get; set; }
        public ContractsSettingViewModel Setting { get; set; }
        public ContractsIndexColumnsModel Columns { get; set; }

        public string SearchText { get; set; }

        

    }

    

    public sealed class ContractsIndexRowModel
    {

        public ContractsIndexRowModel()
        {
            SelectedShowStatus = 10;
            IsInNoticeOfRemoval = false;
        }

        public bool IsInNoticeOfRemoval { get; set; }
        public int SelectedShowStatus { get; set; }

        public int ContractId { get; set; }

        public int CaseNumber { get; set; }

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
            Columns = new List<ContractsSettingRowViewModel>();
            SelectedShowStatus = 10;
        }

        public Customer Customer { get; private set; }
        public List<ContractsIndexRowModel> Data { get; set; }
        public List<ContractsSettingRowViewModel> Columns { get; set; }
        public ColSortModel SortBy { get; set; }

        public int SelectedShowStatus { get; set; }

        

    }

    public sealed class ColSortModel
    {
        public ColSortModel(string columnName, bool isAsc)
        {
            ColumnName = columnName;
            IsAsc = isAsc;
        }

        public string ColumnName { get; private set; }

        public bool IsAsc { get; private set; }

    }
}