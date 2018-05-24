using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Models.Contract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.BusinessData.Models.Contract;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Web.Infrastructure.Tools;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Web.Infrastructure.Mvc;
using DH.Helpdesk.Services.BusinessLogic.Contracts;
using DH.Helpdesk.Web.Components.Contracts;
using DH.Helpdesk.Services.BusinessLogic.Mappers.Cases;
using DH.Helpdesk.Services.Services.Grid;
using DH.Helpdesk.Web.Infrastructure.ActionFilters;
using DH.Helpdesk.Web.Infrastructure.Attributes;
using DH.Helpdesk.Web.Infrastructure.CaseOverview;
using DH.Helpdesk.Web.Infrastructure.Grid;
using DH.Helpdesk.Web.Models.Case;
using DH.Helpdesk.Web.Models.Case.Output;


namespace DH.Helpdesk.Web.Controllers
{
    public class ContractsController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IContractCategoryService _contractCategoryService;
        private readonly ICustomerService _customerService;
        private readonly IContractService _contractService;
        private readonly ISupplierService _supplierService;
        private readonly IDepartmentService _departmentService;
        private readonly ITemporaryFilesCache _userTemporaryFilesStorage;
        private readonly GridSettingsService _gridSettingsService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly CaseOverviewGridSettingsService _caseOverviewSettingsService;

        public ContractsController(
            IUserService userService,
            IContractCategoryService contractCategoryService,
            ICustomerService customerService,
            IContractService contractService,
            ISupplierService supplierService,
            IDepartmentService departmentService,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory,
            GridSettingsService gridSettingsService,
            ICaseFieldSettingService caseFieldSettingService,
            CaseOverviewGridSettingsService caseOverviewSettingsService,
            IMasterDataService masterDataService)

            : base(masterDataService)
        {
            this._userService = userService;
            this._contractCategoryService = contractCategoryService;
            this._contractService = contractService;
            this._customerService = customerService;
            this._supplierService = supplierService;
            this._departmentService = departmentService;
            this._userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Contracts);
            _gridSettingsService = gridSettingsService;
            _caseFieldSettingService = caseFieldSettingService;
            _caseOverviewSettingsService = caseOverviewSettingsService;
        }

        [UserPermissions(UserPermission.ContractPermission)]
        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            //var accountAcctivity = new AccountActivity { Customer_Id = customer.Id };
            var model = this.CreateInputViewModel(customerId);

            return this.View(model);
        }

        //
        // GET: /Contract/
        [HttpGet]
        [UserPermissions(UserPermission.ContractPermission)]
        public ActionResult Index()
        {
            var customer = _customerService.GetCustomer(SessionFacade.CurrentCustomer.Id);
            var sortModel = new ColSortModel(EnumContractFieldSettings.Number, true);

            var filterModel = GetSearchFilter(customer.Id);
            var searchResults = GetSearchResultsModel(filterModel, sortModel);

            var model =
                new ContractIndexViewModel(customer)
                {
                    SearchFilterModel = GetContractsSearchFilterModel(customer),
                    Setting = GetSettingsModel(customer.Id),
                    SearchResults = searchResults
                };

            return this.View(model);
        }

        [HttpPost]
        public ActionResult SortBy(int customerId, string colName, bool isAsc)
        {
            var filterModel = GetSearchFilter(customerId);

            var sortModel = new ColSortModel(colName, isAsc);
            var model = GetSearchResultsModel(filterModel, sortModel);

            return PartialView("_ContractsIndexRows", model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public PartialViewResult Search(ContractsSearchInputData data)
        {
            var filter = new ContractsSearchFilter(data.CustomerId)
            {
                SelectedContractCategories = data.Categories,
                SelectedSuppliers = data.Suppliers,
                SelectedResponsibles = data.ResponsibleUsers,
                SelectedDepartments = data.Departments,
                State = data.ShowContracts,

                StartDateTo = data.StartDateTo,
                StartDateFrom = data.StartDateFrom,

                EndDateTo = data.EndDateTo,
                EndDateFrom = data.EndDateFrom,

                NoticeDateTo = data.NoticeDateTo,
                NoticeDateFrom = data.NoticeDateFrom,

                SearchText = data.SearchText
            };
            
            //save filter in session
            SessionFacade.CurrentContractsSearch = filter;

            var sortModel =
                string.IsNullOrEmpty(data.SortColName)
                    ? new ColSortModel(EnumContractFieldSettings.Number, true)
                    : new ColSortModel(data.SortColName, data.SortAsc);

            var searchResults = GetSearchResultsModel(filter, sortModel);

            return PartialView("_ContractsIndexRows", searchResults);
        }

        private ContractsSearchFilter GetSearchFilter(int customerId)
        {
            var filterModel = SessionFacade.CurrentContractsSearch;
            if (filterModel == null)
            {
                filterModel = new ContractsSearchFilter(customerId)
                {
                    State = ContractStatuses.Active
                };

                SessionFacade.CurrentContractsSearch = filterModel;
            }
            return filterModel;
        }

        private ContractsSearchResultsModel GetSearchResultsModel(ContractsSearchFilter selectedFilter, ColSortModel sort)
        {
            var customerId = selectedFilter.CustomerId;
            var customer = _customerService.GetCustomer(customerId);

            var model = new ContractsSearchResultsModel(customer);

            //Configuring Contract cases popup
            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);
            var columnsSettingsModel = _caseOverviewSettingsService.GetSettings(
                customerId,
                SessionFacade.CurrentUser.UserGroupId,
                SessionFacade.CurrentUser.Id,
                GridSettingsService.CASE_CONTRACT_CASES_GRID,
                caseFieldSettings);
            var gridSettings = _gridSettingsService.GetForCustomerUserGrid(
                customerId,
                SessionFacade.CurrentUser.UserGroupId,
                SessionFacade.CurrentUser.Id,
                GridSettingsService.CASE_CONTRACT_CASES_GRID);
            model.ContractCases = new JsonCaseIndexViewModel
            {
                PageSettings = new PageSettingsModel
                {
                    gridSettings = JsonGridSettingsMapper.ToJsonGridSettingsModel(
                        gridSettings,
                        SessionFacade.CurrentCustomer.Id,
                        columnsSettingsModel.AvailableColumns.Count(),
                        new[] {"5", "10", "15"}),
                    messages = new Dictionary<string, string>()
                    {
                        {"information", Translation.GetCoreTextTranslation("Information")},
                        {
                            "records_limited_msg",
                            Translation.GetCoreTextTranslation("Antal ärende som visas är begränsade till 500.")
                        },
                    }
                },
                // default values for search filter
                CaseSearchFilterData = new CaseSearchFilterData
                {
                    filterCustomerId = SessionFacade.CurrentCustomer.Id,
                    caseSearchFilter = new CaseSearchFilter
                    {
                        CaseType = 0,
                        ProductArea = string.Empty,
                        Category = string.Empty,
                        CaseFilterFavorite = string.Empty,
                        FreeTextSearch = string.Empty,
                        CaseProgress = "-1"
                    },
                    CaseInitiatorFilter = string.Empty,
                    lstfilterPerformer = new int[0],
                    SearchInMyCasesOnly = false
                }
            };

            //run search
            var allContracts = SearchContracts(selectedFilter);
         
            var settings = GetSettingsModel(customer.Id);

            model.Columns = settings.SettingRows.Where(s => s.ShowInList).ToList();
            foreach (var col in model.Columns)
            {
                col.SetOrder();
            }
            model.Columns = model.Columns.OrderBy(s => s.VirtualOrder).ToList();
            model.SelectedShowStatus = selectedFilter.State;

            var showStatus = selectedFilter.State;
            
            foreach (var con in allContracts)
            {
                var isInNoticeOfRemoval = IsInNoticeOfRemoval(con.Finished, con.NoticeDate);
                var isInFollowUp = IsInFollowUp(con.Finished, con.ContractStartDate, con.ContractEndDate, con.FollowUpInterval);

                if ((showStatus == ContractStatuses.All) ||
                    (showStatus != ContractStatuses.InNoticeOfRemoval && showStatus != ContractStatuses.ToFollowUp) ||
                    (showStatus == ContractStatuses.InNoticeOfRemoval && isInNoticeOfRemoval) ||
                    (showStatus == ContractStatuses.ToFollowUp && isInFollowUp))
                {
                    var latestLog = con.ContractLogs.Any(x => x.Case_Id.HasValue)
                        ? con.ContractLogs.Where(x => x.Case_Id.HasValue).OrderByDescending(l => l.CreatedDate).FirstOrDefault()
                        : null;

                    var caseNumbers = con.ContractLogs.Where(x => x.Case_Id.HasValue).Select(c => c.Case.CaseNumber)
                        .ToList();
                    var hasMultipleCases = caseNumbers.Count > 1;
                    var latestCase = latestLog?.Case;

                    model.Data.Add(new ContractsSearchRowModel
                    {
                        ContractId = con.Id,
                        ContractNumber = con.ContractNumber,
                        ContractEndDate = con.ContractEndDate,
                        ContractCase = latestCase != null ? new ContractCase
                        {
                            CaseId = latestCase.Id,
                            CaseNumber = (int)latestCase.CaseNumber,
                            CaseIcon = CasesMapper.GetCaseIcon(latestCase.FinishingDate, latestCase.ApprovedDate, latestCase.CaseType.RequireApproving),
                            HasMultiplyCases = hasMultipleCases,
                            CaseNumbers = caseNumbers.Select(c => c.ToString(CultureInfo.InvariantCulture)).ToList()
                        } : new ContractCase { CaseNumber = 0 },
                        ContractStartDate = con.ContractStartDate,
                        Finished = con.Finished,
                        Running = con.Running,
                        FollowUpInterval = con.FollowUpInterval,
                        Info = con.Info,
                        NoticeDate = con.NoticeDate,
                        Supplier = con.Supplier,
                        ContractCategory = con.ContractCategory,
                        Department = con.Department,
                        ResponsibleUser = con.ResponsibleUser,
                        FollowUpResponsibleUser = con.FollowUpResponsibleUser,
                        IsInFollowUp = isInFollowUp,
                        IsInNoticeOfRemoval = isInNoticeOfRemoval
                    });
                }
            }

            model.TotalRowsCount = model.Data.Count();
            model.Data = SortData(model.Data, sort);
            model.SearchSummary = BuildSearchSummary(model.Data);
            model.SortBy = sort;

            return model;
        }

        //todo: implement searching in db with filter instead of code
        private IList<Contract> SearchContracts(ContractsSearchFilter filter)
        {
            var customerId = filter.CustomerId;
            var allContracts = _contractService.GetContractsNotFinished(customerId);
            var selectedStatus = filter.State; 

            if (filter.SelectedContractCategories.Any())
                allContracts = allContracts.Where(t => filter.SelectedContractCategories.Contains(t.ContractCategory_Id)).ToList();

            if (filter.SelectedDepartments.Any())
                allContracts = allContracts.Where(t => filter.SelectedDepartments.Contains(t.Department_Id ?? 0)).ToList();

            if (filter.SelectedResponsibles.Any())
                allContracts = allContracts.Where(t => filter.SelectedResponsibles.Contains(t.ResponsibleUser_Id ?? 0)).ToList();

            if (filter.SelectedSuppliers.Any())
                allContracts = allContracts.Where(t => filter.SelectedSuppliers.Contains(t.Supplier_Id ?? 0)).ToList();

            //filter by state 
            if (selectedStatus == ContractStatuses.Closed) 
            {
                allContracts = allContracts.Where(t => t.Finished == 1).ToList();
            }
            else if (selectedStatus == ContractStatuses.Running) 
            {
                allContracts = allContracts.Where(t => t.Running == 1).ToList();
            }
            else if (selectedStatus != ContractStatuses.All)
            {
                allContracts = allContracts.Where(t => t.Finished == 0).ToList();
            }

            if (filter.StartDateFrom.HasValue)
            {
                allContracts = allContracts.Where(t => t.ContractStartDate >= filter.StartDateFrom.Value).ToList();
            }

            if (filter.StartDateTo.HasValue)
            {
                allContracts = allContracts.Where(t => t.ContractStartDate <= filter.StartDateTo.Value).ToList();
            }

            if (filter.EndDateFrom.HasValue)
            {
                allContracts = allContracts.Where(t => t.ContractEndDate >= filter.EndDateFrom.Value).ToList();
            }

            if (filter.EndDateTo.HasValue)
            {
                allContracts = allContracts.Where(t => t.ContractEndDate <= filter.EndDateTo.Value).ToList();
            }

            if (filter.NoticeDateFrom.HasValue)
            {
                allContracts = allContracts.Where(t => t.NoticeDate >= filter.NoticeDateFrom.Value).ToList();
            }

            if (filter.NoticeDateTo.HasValue)
            {
                allContracts = allContracts.Where(t => t.NoticeDate <= filter.NoticeDateTo.Value).ToList();
            }

            //search text
            if (!string.IsNullOrEmpty(filter.SearchText)) //todo: sql injection
                allContracts = allContracts.Where(t => t.ContractNumber.Contains(filter.SearchText)).ToList();

            return allContracts;
        }

        //todo: refactor with a generic approach
        private List<ContractsSearchRowModel> SortData(List<ContractsSearchRowModel> data, ColSortModel sort)
        {
            switch (sort.ColumnName)
            {
                case EnumContractFieldSettings.Number:
                    return sort.IsAsc ? data.OrderBy(d => d.ContractNumber).ToList() : data.OrderByDescending(d => d.ContractNumber).ToList();

                case EnumContractFieldSettings.CaseNumber:
                    return sort.IsAsc ? data.OrderBy(d => d.ContractCase.CaseNumber).ToList() : data.OrderByDescending(d => d.ContractCase.CaseNumber).ToList();

                case EnumContractFieldSettings.Category:
                    return sort.IsAsc ? data.OrderBy(d => d.ContractCategory.Name).ToList() : data.OrderByDescending(d => d.ContractCategory.Name).ToList();

                case EnumContractFieldSettings.Supplier:
                    return sort.IsAsc ? data.OrderBy(t => t.Supplier != null && t.Supplier.Name != null
                            ? t.Supplier.Name : string.Empty).ToList() :
                        data.OrderByDescending(t => t.Supplier != null && t.Supplier.Name != null
                            ? t.Supplier.Name : string.Empty).ToList();

                case EnumContractFieldSettings.Department:
                    return sort.IsAsc ? data.OrderBy(d => d.Department.DepartmentName).ToList() : data.OrderByDescending(d => d.Department.DepartmentName).ToList();

                case EnumContractFieldSettings.ResponsibleUser:
                    return sort.IsAsc ? data.OrderBy(t => t.ResponsibleUser != null && t.ResponsibleUser.SurName != null
                            ? t.ResponsibleUser.SurName : string.Empty).ToList() :
                        data.OrderByDescending(t => t.ResponsibleUser != null && t.ResponsibleUser.SurName != null
                            ? t.ResponsibleUser.SurName : string.Empty).ToList();

                case EnumContractFieldSettings.StartDate:
                    return sort.IsAsc ? data.OrderBy(d => d.ContractStartDate).ToList() : data.OrderByDescending(d => d.ContractStartDate).ToList();

                case EnumContractFieldSettings.EndDate:
                    return sort.IsAsc ? data.OrderBy(d => d.ContractEndDate).ToList() : data.OrderByDescending(d => d.ContractEndDate).ToList();

                case EnumContractFieldSettings.NoticeDate:
                    return sort.IsAsc ? data.OrderBy(d => d.NoticeDate).ToList() : data.OrderByDescending(d => d.NoticeDate).ToList();

                //case EnumContractFieldSettings.Filename:
                //    return sort.IsAsc ? data.OrderBy(d => d.).ToList() : data.OrderByDescending(d => d.).ToList();

                case EnumContractFieldSettings.Other:
                    return sort.IsAsc ? data.OrderBy(d => d.Info).ToList() : data.OrderByDescending(d => d.Info).ToList();

                case EnumContractFieldSettings.Running:
                    return sort.IsAsc ? data.OrderBy(d => d.Running).ToList() : data.OrderByDescending(d => d.Running).ToList();

                case EnumContractFieldSettings.Finished:
                    return sort.IsAsc ? data.OrderBy(d => d.Finished).ToList() : data.OrderByDescending(d => d.Finished).ToList();

                case EnumContractFieldSettings.FollowUpField:
                    return sort.IsAsc ? data.OrderBy(d => d.FollowUpInterval).ToList() : data.OrderByDescending(d => d.FollowUpInterval).ToList();

                case EnumContractFieldSettings.ResponsibleFollowUpField:
                    //return sort.IsAsc ? data.OrderBy(d => d.FollowUpResponsibleUser).ToList() : data.OrderByDescending(d => d.FollowUpResponsibleUser).ToList();
                    return sort.IsAsc ? data.OrderBy(t => t.FollowUpResponsibleUser != null && t.FollowUpResponsibleUser.SurName != null
                            ? t.FollowUpResponsibleUser.SurName : string.Empty).ToList() :
                        data.OrderByDescending(t => t.FollowUpResponsibleUser != null && t.FollowUpResponsibleUser.SurName != null
                            ? t.FollowUpResponsibleUser.SurName : string.Empty).ToList();
            }

            return data;
        }

        private ContractsSearchFilterViewModel GetContractsSearchFilterModel(Customer customer)
        {
            var filter = GetSearchFilter(customer.Id);

            var contractCategories = _contractCategoryService.GetContractCategories(customer.Id);

            var model = new ContractsSearchFilterViewModel()
            {
                ContractCategories = contractCategories.Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList(),

                Suppliers = _supplierService.GetActiveSuppliers(customer.Id).Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList(),

                ResponsibleUsers = _userService.GetUsers(customer.Id).Select(u => new SelectListItem()
                {
                    Value = u.Id.ToString(),
                    Text = $"{u.SurName} {u.FirstName}"
                }).ToList(),

                Departments = _departmentService.GetDepartments(customer.Id, ActivationStatus.Active).Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.DepartmentName
                }).ToList(),

                ShowContracts = GetContractStatuses().ToSelectList(),

                SelectedContractCategories = filter.SelectedContractCategories,
                SelectedSuppliers = filter.SelectedSuppliers,
                SelectedResponsibleUsers = filter.SelectedResponsibles,
                SelectedDepartments = filter.SelectedDepartments,
                SelectedState = filter.State,

                NoticeDateFrom = filter.NoticeDateFrom,
                NoticeDateTo = filter.NoticeDateTo,

                StartDateFrom = filter.StartDateFrom,
                StartDateTo = filter.StartDateTo,

                EndDateFrom = filter.EndDateFrom,
                EndDateTo = filter.EndDateTo,

                SearchText = filter.SearchText
            };

            return model;
        }

        private ContractsSearchSummary BuildSearchSummary(List<ContractsSearchRowModel> rows)
        {
            var summary = new ContractsSearchSummary
            {
                TotalCases = rows.Count(),
                RunningCases = rows.Count(z => z.Running == 1),
                OnGoingCases = rows.Count(z => z.Finished == 0),
                FinishedCases = rows.Count(z => z.Finished == 1),
                ContractNoticeOfRemovalCount = rows.Count(row => IsInNoticeOfRemoval(row.Finished, row.NoticeDate)),
                ContractFollowUpCount = rows.Count(row => IsInFollowUp(row.Finished, row.ContractStartDate, row.ContractEndDate, row.FollowUpInterval))

            };
            return summary;
        }

        private bool IsInFollowUp(int finished, DateTime? contractStartDate, DateTime? contractEndDate, int followUpInterval)
        {
            var flag = false;
            var i = 0;
            
            var today = DateTime.Today;

            if (finished == 0 && followUpInterval > 0)
            {
                var endDate = (contractEndDate ?? today.AddMonths(1)).Date;
                
                if (contractStartDate.HasValue)
                {
                    var startDate = contractStartDate.Value.Date;

                    while (startDate < endDate) 
                    {
                        if (today.AddMonths(1) > startDate && startDate < today)
                        {
                            flag = true;
                            break;
                        }

                        startDate = startDate.AddMonths(followUpInterval).Date;

                        i = i + 1;
                        if (i > 100)
                        {
                            break;
                        }
                    } 
                }
            }

            return flag;
        }

        private bool IsInNoticeOfRemoval(int finished, DateTime? noticeDateTime)
        {
            var today = DateTime.Today;
            
            if (finished == 0 && noticeDateTime.HasValue)
            {
                var noticeDate = noticeDateTime.Value.Date;
                
                if (noticeDate < today || noticeDate <= today.AddMonths(1))
                {
                    return true;
                }
            }
            return false;
        }

        private ContractViewInputModel CreateInputViewModel(int customerId)
        {
            var model = new ContractViewInputModel();
            var contractFields = this.GetSettingsModel(customerId);
            var contractcategories = _contractCategoryService.GetContractCategories(customerId).OrderBy(a => a.Name).ToList();
            var suppliers = _supplierService.GetActiveSuppliers(customerId);
            var departments = _departmentService.GetDepartments(customerId);
            var users = _userService.GetCustomerActiveUsers(customerId);

            var emptyChoice = new SelectListItem() { Selected = true, Text = "", Value = string.Empty };

            //if (contractFields != null)
            //{          
            //model.NoticeTime.Insert(0, emptyChoice);
            model.NoticeTimes.Add(new SelectListItem() { Selected = false, Text = "1 " + Translation.GetCoreTextTranslation("månad"), Value = "1" });

            for (int i = 2; i <= 12; i++)
            {
                model.NoticeTimes.Add(new SelectListItem() { Selected = false, Text = i.ToString() + " " + Translation.GetCoreTextTranslation("månader"), Value = i.ToString() });
            }

            model.SettingsModel = contractFields.SettingRows.Select(conf => new ContractsSettingRowViewModel
            {
                Id = conf.Id,
                ContractField = conf.ContractField.ToLower(),
                ContractFieldLable = conf.ContractFieldLable,
                ContractFieldLable_Eng = conf.ContractFieldLable_Eng,
                Show = conf.Show,
                ShowInList = conf.ShowInList,
                Required = conf.Required
            }).ToList();

            model.ContractFiles = new List<ContractFileViewModel>();
            model.ContractFileKey = Guid.NewGuid().ToString();

            model.ContractCategories = contractcategories.Select(x => new SelectListItem
            {
                Selected = (x.Id == model.CategoryId ? true : false),
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            model.ContractCategories.Insert(0, emptyChoice);

            model.Suppliers = suppliers.Select(x => new SelectListItem
            {
                Selected = (x.Id == model.SupplierId ? true : false),
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            model.Suppliers.Insert(0, emptyChoice);

            //Departments
            model.Departments = departments.Select(x => new SelectListItem
            {
                Selected = (x.Id == model.DepartmentId ? true : false),
                Text = x.DepartmentName,
                Value = x.Id.ToString()
            }).ToList();
            model.Departments.Insert(0, emptyChoice);
            
            //ResponsibleUsers
            model.ResponsibleUsers = users.Select(x => new SelectListItem
            {
                Selected = (x.Id == model.ResponsibleUserId ? true : false),
                Text = x.SurName + " " + x.FirstName,
                Value = x.Id.ToString()
            }).ToList();
            model.ResponsibleUsers.Insert(0, emptyChoice);

            //FollowUpIntervals
            var followUpIntervals = GetFollowUpIntervals();
            model.FollowUpIntervals.AddRange(followUpIntervals.ToSelectList());
            model.FollowUpIntervals.Insert(0, emptyChoice);

            //FollowUpResponsibleUsers
            model.FollowUpResponsibleUsers = users.Select(x => new SelectListItem
            {
                Selected = (x.Id == model.FollowUpResponsibleUserId ? true : false),
                Text = x.SurName + " " + x.FirstName,
                Value = x.Id.ToString()
            }).ToList();
            model.FollowUpResponsibleUsers.Insert(0, emptyChoice);

            return model;
        }

        private IDictionary<int, string> GetFollowUpIntervals()
        {
            return new Dictionary<int, string>
            {
                {1,  Translation.GetCoreTextTranslation("månadsvis")},
                {3,  Translation.GetCoreTextTranslation("kvartalsvis")},
                {4,  Translation.GetCoreTextTranslation("tertialvis")},
                {6,  Translation.GetCoreTextTranslation("halvårsvis")},
                {12, Translation.GetCoreTextTranslation("årsvis")}
            };
        }

        private IDictionary<int, string> GetContractStatuses()
        {
            var dic = new Dictionary<int, string>
            {
                {ContractStatuses.Active, $"  {Translation.GetCoreTextTranslation("Pågående")}"},
                {ContractStatuses.ToFollowUp, $"  {Translation.GetCoreTextTranslation("För uppföljning")}"},
                {ContractStatuses.InNoticeOfRemoval, $"  {Translation.GetCoreTextTranslation("För uppsägning")}"},
                {ContractStatuses.Running, $"  {Translation.GetCoreTextTranslation("Löpande")}"},
                {ContractStatuses.Closed, $"  {Translation.GetCoreTextTranslation("Avslutade")}"},
                {ContractStatuses.All, $"  {Translation.GetCoreTextTranslation("Alla")}"}
            };
            return dic;
        }

        [HttpPost]
        [UserPermissions(UserPermission.ContractPermission)]
        public ActionResult New(ContractViewInputModel contractInput, string actiontype, string contractFileKey)
        {
            var temporaryFiles = _userTemporaryFilesStorage.FindFiles(contractFileKey);
            
            var files = temporaryFiles.Select(f => $"{StringTags.Add}{f.Name}").ToList();
            var cId = this.SaveContract(contractInput, files);

            var contractFiles = temporaryFiles.Select(f => new ContractFileModel(0, cId, f.Content, null, MimeMapping.GetMimeMapping(f.Name), f.Name, DateTime.UtcNow, DateTime.UtcNow, Guid.NewGuid())).ToList();

            foreach (var contractFile in contractFiles)
            {
                this._contractService.SaveContracFile(contractFile);
                _userTemporaryFilesStorage.DeleteFile(contractFile.FileName, contractInput.ContractFileKey);
            }

            if (actiontype != "Spara och stäng")
            {
                return this.RedirectToAction("Edit", "Contracts", new { id = cId });
            }

            return RedirectToAction("index", "Contracts");
        }

        [HttpPost]
        [UserPermissions(UserPermission.ContractPermission)]
        [BadRequestOnNotValid]
        public JsonResult SaveContractFieldsSetting(List<ContractsSettingRowData> data)
        {
            if (SessionFacade.CurrentCustomer == null)
            {
                return Json(new
                {
                    state = false,
                    message = "Session Timeout. Please refresh the page!"
                });
            }

            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var allFieldsSettingRows = _contractService.GetContractsSettingRows(currentCustomerId);

            try
            {
                var contractSettingModels = new List<ContractsSettingRowModel>();
                var now = DateTime.Now;

                foreach (var rowData in data)
                {
                    var oldRow = allFieldsSettingRows.FirstOrDefault(s => s.ContractField.ToLower() == rowData.ContractField);
                    var createdDate = oldRow?.CreatedDate ?? now;
                    var id = oldRow?.Id ?? 0;
                    var contractSettingModel = new ContractsSettingRowModel(
                                                    id,
                                                    currentCustomerId,
                                                    rowData.ContractField,
                                                    rowData.Show,
                                                    rowData.ShowInList,
                                                    rowData.CaptionSv ?? string.Empty,
                                                    rowData.CaptionEng ?? string.Empty,
                                                    rowData.Required,
                                                    createdDate,
                                                    now);

                    contractSettingModels.Add(contractSettingModel);
                }

                _contractService.SaveContractSettings(contractSettingModels);
            }
            catch (Exception ex)
            {
                return Json(new { state = false, message = ex.Message });
            }

            return Json(new { state = true, message = Translation.GetCoreTextTranslation("Saved success!") });
        }

        [HttpGet]
        public ActionResult ContractHistory(int id)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
            var settings = GetCustomerSettings(customerId);
            var fieldSettings = _contractService.GetContractsSettingRows(customerId);
            var contractHistory = _contractService.GetContractractHistoryList(id);
            var followUpIntervals = GetFollowUpIntervals();

            var model = GetHistoryDiff(contractHistory, userTimeZone, settings, fieldSettings, followUpIntervals);

            return PartialView(model);
        }

        private IList<ContractHistoryFieldsDiff> GetHistoryDiff(IList<ContractHistoryFull> items,
            TimeZoneInfo userTimeZone, Setting settings, IList<ContractsSettingRowModel> fieldSettings,
            IDictionary<int, string> followUpIntervals)
        {
            var diffList = new List<ContractHistoryFieldsDiff>();
            var isUserFirstLastNameRepresentation = settings.IsUserFirstLastNameRepresentation == 1;

            var formatter = new ContractHistoryFormatter(userTimeZone, followUpIntervals, isUserFirstLastNameRepresentation);
            var historyBuilder =
                new ContractHistoryFieldsDiffBuilder(formatter, fieldSettings, SessionFacade.CurrentLanguageId);

            ContractHistoryFull prev = null;
            foreach (var item in items)
            {
                var firstName = item.CreatedByUser.FirstName;
                var lastName = item.CreatedByUser.SurName;
                var registeredBy = isUserFirstLastNameRepresentation
                    ? $"{firstName} {lastName}"
                    : $"{lastName} {firstName}";

                var historyDiff = historyBuilder.BuildHistoryDiff(prev, item);

                var diff = new ContractHistoryFieldsDiff()
                {
                    Modified = TimeZoneInfo.ConvertTimeFromUtc(item.CreatedAt, userTimeZone),
                    RegisteredBy = registeredBy,
                    FieldsDiff = historyDiff,
                    //Emails = //todo: log emails
                };

                if (diff.FieldsDiff.Any())
                {
                    diffList.Add(diff);
                    prev = item;
                }
            }

            diffList.Reverse();
            return diffList;
        }

        private ContractsSettingViewModel GetSettingsModel(int customerId)
        {
            var allFieldsSettingRows = _contractService.GetContractsSettingRows(customerId);

            var model = new ContractsSettingViewModel
            {
                CustomerId = customerId,
                CurrentLanguage = SessionFacade.CurrentLanguageId,
                Languages = new Dictionary<int, string> {{1, "SV"}, {2, "EN"}}.ToSelectList(),
                SettingRows =
                    allFieldsSettingRows.Select(s => new ContractsSettingRowViewModel
                        {
                            Id = s.Id,
                            ContractField = s.ContractField.ToLower(),
                            ContractFieldLable = s.ContractFieldLable,
                            ContractFieldLable_Eng = s.ContractFieldLable_Eng,
                            Show = s.show,
                            ShowInList = s.showInList,
                            Required = s.reguired
                        }).ToList()
            };

            return model;
        }

        [HttpPost]
        [UserPermissions(UserPermission.ContractPermission)]
        public void UploadContractFile(string id, string name)
        {
            var uploadedFile = this.Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            if (GuidHelper.IsGuid(id))
            {
                if (this._userTemporaryFilesStorage.FileExists(name, id))
                {
                    _userTemporaryFilesStorage.DeleteFile(name, id);
                    //throw new HttpException((int)HttpStatusCode.Conflict, null); because it take a long time.
                }
                this._userTemporaryFilesStorage.AddFile(uploadedData, name, id);
            }
        }


        [HttpGet]
        [UserPermissions(UserPermission.ContractPermission)]
        public UnicodeFileContentResult DownloadFile(string id, string fileName, string filePlace)
        {
            byte[] fileContent;
            if (id == "0")
                fileContent = _userTemporaryFilesStorage.GetFileContent(fileName.Trim(), filePlace, "");
            else
            {
                fileContent = _contractService.GetContractFile(int.Parse(id)).Content;
            }

            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", fileName));

            return new UnicodeFileContentResult(fileContent, fileName);

        }

        [HttpGet]
        [UserPermissions(UserPermission.ContractPermission)]
        public JsonResult DeleteContractFile(string id, string fileName, string filePlace)
        {
            try
            {
                if (id == "0")
                {
                    _userTemporaryFilesStorage.DeleteFile(fileName.Trim(), filePlace);
                }
                else
                {
                    var fileId = int.Parse(id);
                    var fileInfo = _contractService.GetContractFile(fileId);

                    this._contractService.DeleteContractFile(fileId);
                    
                    //save contract history 
                    var contract = _contractService.GetContract(fileInfo.Contract_Id);
                    _contractService.SaveContractHistory(contract, new List<string> { StringTags.Delete + fileInfo.FileName });
                }

                return Json(new { result = true, message = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }

        }

        [HttpGet]
        [UserPermissions(UserPermission.ContractPermission)]
        public JsonResult GetAllFiles(string id, int cId)
        {
            string[] fileNames = { };
            var tempFiles = _userTemporaryFilesStorage.FindFiles(id);

            if (tempFiles.Any())
            {
                fileNames = tempFiles.Select(f => f.Name).ToArray();
            }

            return Json(fileNames, JsonRequestBehavior.AllowGet);
        }

        private int SaveContract(ContractViewInputModel contractInput, List<string> files)
        {
            var cId = 0;
            if (contractInput != null)
            {
                var contractInputToSave = new ContractInputModel
                    (
                     contractInput.ContractId,
                     SessionFacade.CurrentCustomer.Id,
                     SessionFacade.CurrentUser.Id,
                     contractInput.CategoryId,
                     contractInput.SupplierId,
                     contractInput.DepartmentId,
                     contractInput.ResponsibleUserId,
                     contractInput.FollowUpResponsibleUserId,
                     contractInput.ContractNumber,
                     contractInput.ContractStartDate,
                     contractInput.ContractEndDate,
                     contractInput.NoticeTimeId,
                     contractInput.Finished,
                     contractInput.Running,
                     contractInput.FollowUpIntervalId,
                     contractInput.Other,
                     contractInput.NoticeDate,
                     DateTime.Now,
                     DateTime.Now,
                     Guid.Empty,
                     files);

                cId = this._contractService.SaveContract(contractInputToSave);
            }

            return cId;
        }

        private List<ContractFileViewModel> CreateContractFilesModel(int contractId, string CFileKey)
        {
            var contractFiles = this._contractService.GetContractFiles(contractId);

            var contractFilesInput = contractFiles.Select(conf => new ContractFileViewModel()
            {
                Id = conf.Id,
                Contract_Id = conf.Contract_Id,
                ArchivedContractFile_Id = conf.ArchivedContractFile_Id,
                FileName = conf.FileName,
                ArchivedDate = conf.ArchivedDate,
                Content = conf.Content,
                ContractType = conf.ContentType,
                ContractFileKey = CFileKey,
                ContractFileGuid = conf.ContractFileGuid,
                CreatedDate = conf.CreatedDate
            }).ToList();
            return contractFilesInput;
        }

        private List<string> ContractFilesNames(int contractId)
        {
            List<string> contractFilesNames = null;
            var contractFiles = this._contractService.GetContractFiles(contractId);
            contractFilesNames = contractFiles.Select(conf => conf.FileName).ToList();

            return contractFilesNames;
        }

        private ContractFileModel findContractFile(int fileId, string fileName)
        {
            var contractFile = this._contractService.GetContractFile(fileId);

            return contractFile;
        }

        private List<WebTemporaryFile> TransferContractFiles(string contractFilesKey, int contractId)
        {
            var filesToAdd = new List<WebTemporaryFile>();
            var contractFiles = this._contractService.GetContractFiles(contractId);
            if (contractFiles != null)
            {
                foreach (var f in contractFiles)
                {
                    filesToAdd.Add(new WebTemporaryFile(f.Content, f.FileName));
                    this._userTemporaryFilesStorage.AddFile(f.Content, f.FileName, contractFilesKey);
                }
            }
            return filesToAdd;
        }
        //
        // GET: /Contract/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Contract/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Contract/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Contract/Edit/5
        [UserPermissions(UserPermission.ContractPermission)]
        public ActionResult Edit(int id)
        {
            var contractFields = this.GetSettingsModel(SessionFacade.CurrentCustomer.Id);
            var contract = this._contractService.GetContract(id);


            if (contract == null)
                return new HttpNotFoundResult("No contract found...");
            else
            {
                var contractsExistingFiles = this._contractService.GetContractFiles(id);
                var changedbyUser = this._userService.GetUser(contract.ChangedByUser_Id);

                var contractEditInput = CreateInputViewModel(SessionFacade.CurrentCustomer.Id);

                contractEditInput.ContractId = contract.Id;
                contractEditInput.CategoryId = contract.ContractCategory_Id;
                contractEditInput.SupplierId = Convert.ToInt32(contract.Supplier_Id);
                contractEditInput.DepartmentId = Convert.ToInt32(contract.Department_Id);
                contractEditInput.ResponsibleUserId = Convert.ToInt32(contract.ResponsibleUser_Id);
                contractEditInput.FollowUpIntervalId = contract.FollowUpInterval;
                contractEditInput.FollowUpResponsibleUserId = Convert.ToInt32(contract.FollowUpResponsibleUser_Id);
                contractEditInput.ContractNumber = contract.ContractNumber;
                contractEditInput.ContractStartDate = contract.ContractStartDate;
                contractEditInput.ContractEndDate = contract.ContractEndDate;
                contractEditInput.NoticeTimeId = contract.NoticeTime;
                contractEditInput.Finished = Convert.ToBoolean(contract.Finished);
                contractEditInput.Running = Convert.ToBoolean(contract.Running);
                contractEditInput.Other = contract.Info;
                contractEditInput.NoticeDate = contract.NoticeDate;
                contractEditInput.ChangedByUser = changedbyUser.SurName + " " + changedbyUser.FirstName;
                contractEditInput.CreatedDate = contract.CreatedDate.ToShortDateString();
                contractEditInput.ChangedDate = contract.ChangedDate.ToShortDateString();
                contractEditInput.ContractFiles = CreateContractFilesModel(contract.Id, contractEditInput.ContractFileKey);

                return this.View(contractEditInput);
            }
        }


        //
        // POST: /Contract/Edit/5

        [HttpPost]
        [UserPermissions(UserPermission.ContractPermission)]
        public ActionResult Edit(int id, ContractViewInputModel contractInput, string actiontype, string contractFileKey)
        {
            try
            {
                var temporaryFiles = _userTemporaryFilesStorage.FindFiles(contractFileKey);
                var files = temporaryFiles.Select(f => $"{StringTags.Add}{f.Name}").ToList();

                var cId = SaveContract(contractInput, files);
                
                var contractsExistingFiles = this._contractService.GetContractFiles(id);
                var contractFiles = temporaryFiles.Select(f => new ContractFileModel(0, cId, f.Content, null, MimeMapping.GetMimeMapping(f.Name), f.Name, DateTime.UtcNow, DateTime.UtcNow, Guid.NewGuid())).ToList();

                foreach (var contractFile in contractFiles)
                {
                    this._contractService.SaveContracFile(contractFile);
                    _userTemporaryFilesStorage.DeleteFile(contractFile.FileName, contractInput.ContractFileKey);
                }

                if (actiontype == null)
                {
                    return this.RedirectToAction("Edit", "Contracts", new { id = cId });
                }
                else
                    return this.RedirectToAction("index", "Contracts");
            }
            catch (Exception ex)
            {
                return this.RedirectToAction("index", "Contracts");
            }
        }

        //
        // GET: /Contract/Delete/5

        public ActionResult Delete(int id)
        {
            return this.RedirectToAction("index", "Contracts");
            //return View();
        }

        //
        // POST: /Contract/Delete/5

        [HttpPost]
        [UserPermissions(UserPermission.ContractPermission)]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var contract = this._contractService.GetContract(id);

                this._contractService.DeleteContract(contract);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #region Helper Methods

        private bool TryParseDate(string value)
        {
            DateTime dt1;
            bool res = DateTime.TryParse(value, out dt1);
            return res;
        }

        #endregion
    }
}
