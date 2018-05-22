using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.OldComponents;


namespace DH.Helpdesk.Web.Models.Contract
{
    public class ContractIndexViewModel
    {
        public ContractIndexViewModel(Customer customer)
        {
            Customer = customer;
            Columns = new ContractsIndexColumnsModel(customer);
            SearchResults = new ContractsSearchResultsModel(customer);
        }
        
        public Customer Customer { get; private set; }
        
        public ContractsSettingViewModel Setting { get; set; }
        public ContractsIndexColumnsModel Columns { get; set; }
        public ContractsSearchResultsModel SearchResults { get; set; }

        public ContractsSearchFilterViewModel SearchFilterModel { get; set; }
    }

    public class ContractsSearchFilterViewModel
    {
        public ContractsSearchFilterViewModel()
        {
            SelectedContractCategories = new List<int>();
            SelectedSuppliers = new List<int>();
            SelectedResponsibleUsers = new List<int>();
            SelectedDepartments = new List<int>();
        }

        public IList<SelectListItem> ContractCategories { get; set; }
        public IList<SelectListItem> Suppliers { get; set; }
        public IList<SelectListItem> ResponsibleUsers { get; set; }
        public IList<SelectListItem> Departments { get; set; }
        public IList<SelectListItem> ShowContracts { get; set; }

        public DateTime? NoticeDateFrom { get; set; }
        public DateTime? NoticeDateTo { get; set; }

        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }

        public DateTime? EndDateFrom { get; set; }
        public DateTime? EndDateTo { get; set; }

        public string SearchText { get; set; }
        
        //selected dropdown values
        public List<int> SelectedContractCategories { get; set; }
        public List<int> SelectedSuppliers { get; set; }
        public List<int> SelectedResponsibleUsers { get; set; }
        public List<int> SelectedDepartments { get; set; }
        public int SelectedState { get; set; }
    }

    public class ContractsSearchSummary
    {
        public int TotalCases { get; set; }
        public int OnGoingCases { get; set; }
        public int FinishedCases { get; set; }
        public int ContractNoticeOfRemovalCount { get; set; }
        public int ContractFollowUpCount { get; set; }
        public int RunningCases { get; set; }
    }

    public sealed class ContractsSearchRowModel
    {
        public ContractsSearchRowModel()
        {
            IsInNoticeOfRemoval = false;
            IsInFollowUp = false;
            ContractCase = new ContractCase();
        }

        public bool IsInNoticeOfRemoval { get; set; }

        public bool IsInFollowUp { get; set; }

        public int SelectedShowStatus { get; set; }

        public int ContractId { get; set; }

        public ContractCase ContractCase { get; set; }

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

    public sealed class ContractsSearchResultsModel
    {
        public ContractsSearchResultsModel(Customer customer)
        {
            Data = new List<ContractsSearchRowModel>();
            Customer = customer;
            Columns = new List<ContractsSettingRowViewModel>();
            SelectedShowStatus = 10;
        }

        public Customer Customer { get; private set; }
        public List<ContractsSearchRowModel> Data { get; set; }
        public List<ContractsSettingRowViewModel> Columns { get; set; }
        public ColSortModel SortBy { get; set; }

        public int SelectedShowStatus { get; set; }
        public int TotalRowsCount { get; set; }
        public ContractsSearchSummary SearchSummary { get; set; }
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

    public sealed class ContractCase
    {
        public int CaseNumber { get; set; }
        public GlobalEnums.CaseIcon CaseIcon { get; set; }
        public bool HasMultiplyCases { get; set; }
    }
}