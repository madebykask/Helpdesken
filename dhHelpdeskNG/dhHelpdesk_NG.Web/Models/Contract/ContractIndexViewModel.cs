using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Extensions;


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
            var dic = new Dictionary<int, string>
            {
                { 1, $"  {Translation.GetCoreTextTranslation("Pågående")}" },
                { 2, $"  {Translation.GetCoreTextTranslation("För uppföljning")}" },
                { 3, $"  {Translation.GetCoreTextTranslation("För uppsägning")}" },
                { 4, $"  {Translation.GetCoreTextTranslation("Löpande")}" },
                { 9, $"  {Translation.GetCoreTextTranslation("Avslutade")}" },
                { 10, $"  {Translation.GetCoreTextTranslation("Alla")}" }
            };

            var items = dic.ToSelectList();
            items.Single(x => x.Value == "10").Selected = true;

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

        public int TotalCases { get; set; }
        public int OnGoingCases { get; set; }

        public int FinishedCases { get; set; }

        public int ContractNoticeOfRemovalCount { get; set; }

        public int ContractFollowUpCount { get; set; }

        public int RunningCases { get; set; }
    }

    public sealed class ContractsIndexRowModel
    {

        public ContractsIndexRowModel()
        {
            SelectedShowStatus = 10;
            IsInNoticeOfRemoval = false;
            IsInFollowUp = false;
        }

        public bool IsInNoticeOfRemoval { get; set; }

        public bool IsInFollowUp { get; set; }
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