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
using DH.Helpdesk.Services.Enums;
using DH.Helpdesk.Services.Services.Grid;
using DH.Helpdesk.Web.Common.Tools.Files;
using DH.Helpdesk.Web.Infrastructure.ActionFilters;
using DH.Helpdesk.Web.Infrastructure.Attributes;
using DH.Helpdesk.Web.Infrastructure.CaseOverview;
using DH.Helpdesk.Web.Infrastructure.Grid;
using DH.Helpdesk.Web.Models.Case;
using DH.Helpdesk.Web.Models.Case.Output;
using System.IO;

namespace DH.Helpdesk.Web.Controllers
{
    public class ContractsController : UserInteractionController 
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
		private readonly IGlobalSettingService _globalSettingService;

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
            IMasterDataService masterDataService,
			IGlobalSettingService globalSettingService)

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
			_globalSettingService = globalSettingService;
        }

        [UserPermissions(UserPermission.ContractPermission)]
        public ActionResult New(int customerId)
        {
            var user = _userService.GetUser(SessionFacade.CurrentUser.Id);
            var model = this.CreateInputViewModel(customerId, user);

            return this.View(model);
        }

        //
        // GET: /Contract/
        [HttpGet]
        [UserPermissions(UserPermission.ContractPermission)]
        public ActionResult Index()
        {
            var user = _userService.GetUser(SessionFacade.CurrentUser.Id);
            var customer = _customerService.GetCustomer(SessionFacade.CurrentCustomer.Id);
            var sortModel = new ColSortModel(EnumContractFieldSettings.Number, true);

            //create missing field settings
            _contractService.CreateMissingFieldSettings(customer.Id);

            var filterModel = GetSearchFilter(customer.Id);
            var searchResults = GetSearchResultsModel(filterModel, sortModel);

            var model =
                new ContractIndexViewModel(customer, user)
                {
                    SearchFilterModel = GetContractsSearchFilterModel(customer, user),
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
                SelectedResponsibleFollowUpUsers = data.ResponsibleFollowUpUsers,
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
            var user = _userService.GetUser(SessionFacade.CurrentUser.Id);
            var filterModel = SessionFacade.CurrentContractsSearch;
            if (filterModel == null)
            {
                filterModel = new ContractsSearchFilter(customerId)
                {
                    State = ContractStatuses.Active
                };
                if (user.CCs.Count > 0)
                {
                    var selectedCategories = new List<int>();
                    foreach(var cc in user.CCs)
                    {
                        selectedCategories.Add(cc.Id);
                    }
                    filterModel.SelectedContractCategories = selectedCategories;
                }

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

            var contractCasesGrid = gridSettings;
            var userSelectedGrid = _gridSettingsService.GetForCustomerUserGrid(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentUser.UserGroupId,
                    SessionFacade.CurrentUser.Id,
                    GridSettingsService.CASE_OVERVIEW_GRID_ID);

            var existingColumns = contractCasesGrid.columnDefs.Where(a => userSelectedGrid.columnDefs.Any(b => b.name == a.name)).ToList();
            var otherColumns = userSelectedGrid.columnDefs.Where(a => !existingColumns.Any(b => b.name == a.name)).ToList();

            var maxColumns = 7;
            if (existingColumns.Count < maxColumns)
            {
                var missingAmount = Math.Abs(maxColumns - existingColumns.Count);
                existingColumns.AddRange(otherColumns.Take(missingAmount));
                contractCasesGrid.columnDefs = existingColumns;
            }

            gridSettings = contractCasesGrid;

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
                        { "information", Translation.GetCoreTextTranslation("Information") },
                        { "records_limited_msg", Translation.GetCoreTextTranslation("Antal ärende som visas är begränsade till 500.") },
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


            //var userDepartments = _departmentService
            //    .GetDepartmentsForUser(customer.Id, SessionFacade.CurrentUser.Id)
            //    .Select(d => d.Id)
            //    .ToList();
            //selectedFilter.SelectedDepartments = (selectedFilter.SelectedDepartments == null || !selectedFilter.SelectedDepartments.Any()) ?
            //    userDepartments :
            //    userDepartments.Intersect(selectedFilter.SelectedDepartments).ToList();

            //run search
            var contractsSearchResults = SearchContracts(selectedFilter);
         
            var settings = GetSettingsModel(customer.Id);

            model.Columns = settings.SettingRows.Where(s => s.ShowInList).ToList();
            foreach (var col in model.Columns)
            {
                col.SetOrder();
            }
            model.Columns = model.Columns.OrderBy(s => s.VirtualOrder).ToList();
            model.SelectedShowStatus = selectedFilter.State;

            var showStatus = selectedFilter.State;
            
            foreach (var contract in contractsSearchResults)
            {
                

                var isInNoticeOfRemoval = IsInNoticeOfRemoval(contract.Finished, contract.NoticeDate);
                var isInFollowUp = IsInFollowUp(contract.Finished, contract.ContractStartDate, contract.ContractEndDate, contract.FollowUpInterval);

                if ((showStatus == ContractStatuses.All) ||
                    (showStatus != ContractStatuses.InNoticeOfRemoval && showStatus != ContractStatuses.ToFollowUp) ||
                    (showStatus == ContractStatuses.InNoticeOfRemoval && isInNoticeOfRemoval) ||
                    (showStatus == ContractStatuses.ToFollowUp && isInFollowUp))
                {
                    var latestLogCase = contract.Cases.Any(x => x.CaseId.HasValue)
                        ? contract.Cases.Where(x => x.CaseId.HasValue).OrderByDescending(l => l.LogCreatedDate).FirstOrDefault()
                        : null;

                    var caseNumbers = contract.Cases.Where(x => x.CaseId.HasValue).Select(c => c.CaseNumber).ToList();
                    var hasMultipleCases = caseNumbers.Count > 1;

                    model.Data.Add(new ContractsSearchRowModel
                    {
                        ContractId = contract.Id,
                        ContractNumber = contract.ContractNumber,
                        ContractEndDate = contract.ContractEndDate,

                        ContractCase = latestLogCase != null ? new ContractCase
                        {
                            CaseId = (int)latestLogCase.CaseId.Value,
                            CaseNumber = (int)latestLogCase.CaseNumber,
                            CaseIcon = CasesMapper.GetCaseIcon(latestLogCase.CaseFinishingDate, latestLogCase.CaseApprovedDate, latestLogCase.CaseTypeRequireApproving),
                            HasMultiplyCases = hasMultipleCases,
                            CaseNumbers = caseNumbers.Select(c => c.ToString(CultureInfo.InvariantCulture)).ToList()
                        } : new ContractCase { CaseNumber = 0 },

                        ContractStartDate = contract.ContractStartDate,
                        Finished = contract.Finished,
                        Running = contract.Running,
                        FollowUpInterval = contract.FollowUpInterval,
                        Info = contract.Info,
                        NoticeDate = contract.NoticeDate,
                        IsInFollowUp = isInFollowUp,
                        IsInNoticeOfRemoval = isInNoticeOfRemoval,
                        Supplier = contract.Supplier,
                        ContractCategory = contract.ContractCategory,
                        Department = contract.Department,
                        ResponsibleUser = contract.ResponsibleUser,
                        FollowUpResponsibleUser = contract.FollowUpResponsibleUser
                    });
                }
            }

            model.TotalRowsCount = model.Data.Count();
            model.Data = SortData(model.Data, sort);
            model.SearchSummary = BuildSearchSummary(model.Data);
            model.SortBy = sort;

            return model;
        }

        private IList<ContractSearchItemData> SearchContracts(ContractsSearchFilter filter)
        {
            var contracts  = _contractService.SearchContracts(filter, SessionFacade.CurrentUser.Id);
            return contracts;
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
                    return sort.IsAsc ? data.OrderBy(d => d.Department.Name).ToList() : data.OrderByDescending(d => d.Department.Name).ToList();

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

                case EnumContractFieldSettings.FollowUp:
                    return sort.IsAsc ? data.OrderBy(d => d.FollowUpInterval).ToList() : data.OrderByDescending(d => d.FollowUpInterval).ToList();

                case EnumContractFieldSettings.ResponsibleFollowUp:
                    //return sort.IsAsc ? data.OrderBy(d => d.FollowUpResponsibleUser).ToList() : data.OrderByDescending(d => d.FollowUpResponsibleUser).ToList();
                    return sort.IsAsc
                        ? data.OrderBy(t => t.FollowUpResponsibleUser?.SurName ?? string.Empty).ToList()
                        : data.OrderByDescending(t => t.FollowUpResponsibleUser?.SurName ?? string.Empty).ToList();
            }

            return data;
        }

        private ContractsSearchFilterViewModel GetContractsSearchFilterModel(Customer customer, User user)
        {
            var filter = GetSearchFilter(customer.Id);
            var contractCategories = _contractCategoryService.GetContractCategories(customer.Id);
            if (user.CCs.Count() > 0)
                contractCategories = user.CCs.ToList();
            
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

                ResponsibleUsers = _userService.GetAvailablePerformersOrUserId(customer.Id).Select(u => new SelectListItem()
                {
                    Value = u.Id.ToString(),
                    Text = $"{u.SurName} {u.FirstName}"
                }).ToList(),

                Departments = _departmentService.GetDepartmentsForUser(customer.Id, SessionFacade.CurrentUser.Id)
                    .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.DepartmentName
                }).ToList(),

                ShowContracts = GetContractStatuses().ToSelectList(),

                SelectedContractCategories = filter.SelectedContractCategories,
                SelectedSuppliers = filter.SelectedSuppliers,
                SelectedResponsibleUsers = filter.SelectedResponsibles,
                SelectedResponsibleFollowUpUsers = filter.SelectedResponsibleFollowUpUsers,
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

        private ContractViewInputModel CreateInputViewModel(int customerId, User user)
        {
            var model = new ContractViewInputModel();
            var contractFields = this.GetSettingsModel(customerId);
            var contractCategories = _contractCategoryService.GetContractCategories(customerId).OrderBy(a => a.Name).ToList();
            if (user.CCs.Count() > 0)
                contractCategories = user.CCs.ToList();

            var suppliers = _supplierService.GetActiveSuppliers(customerId);
            var departments = _departmentService.GetDepartmentsForUser(customerId, SessionFacade.CurrentUser.Id);
            var users = _userService.GetCustomerUsers(customerId);

            var emptyChoice = new SelectListItem() { Selected = true, Text = "", Value = string.Empty };

            //if (contractFields != null)
            //{          
            //model.NoticeTime.Insert(0, emptyChoice);
            model.NoticeTimes.Add(new SelectListItem() { Selected = false, Text = "1 " + Translation.GetCoreTextTranslation("månad"), Value = "1" });

            for (var i = 2; i <= 12; i++)
            {
                model.NoticeTimes.Add(new SelectListItem() { Selected = false, Text = i.ToString() + " " + Translation.GetCoreTextTranslation("månader"), Value = i.ToString() });
            }

            model.SettingsModel = contractFields.SettingRows.Select(conf => new ContractsSettingRowViewModel
            {
                Id = conf.Id,
                ContractField = conf.ContractField.ToLower(),
                ContractFieldLabel = conf.ContractFieldLabel,
                ContractFieldLabel_Eng = conf.ContractFieldLabel_Eng,
                Show = conf.Show,
                ShowInList = conf.ShowInList,
                Required = conf.Required
            }).ToList();

            model.ContractFiles = new List<ContractFileViewModel>();
            model.ContractFileKey = Guid.NewGuid().ToString();

            model.ContractCategories = contractCategories.Select(x => new SelectListItem
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

			model.FileUploadWhiteList = _globalSettingService.GetFileUploadWhiteList();

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
                            ContractFieldLabel = s.ContractFieldLable,
                            ContractFieldLabel_Eng = s.ContractFieldLable_Eng,
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
			var extension = Path.GetExtension(name);
			if (!_globalSettingService.IsExtensionInWhitelist(extension))
			{
				throw new ArgumentException($"File extension is not valid: {name}");
			}

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
                    var contract = _contractService.GetContract(fileInfo.Contract_Id, SessionFacade.CurrentUser.Id);
                    if (contract == null)
                        throw new Exception("No contract found...");

                    if(!HasPermissions(contract))
                        throw new Exception("No permissions...");

                    _contractService.DeleteContractFile(fileId);
                    
                    //save contract history 
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
            var contract = _contractService.GetContract(id, SessionFacade.CurrentUser.Id);

            if (contract == null)
                return new HttpNotFoundResult("No contract found...");
            if(!HasPermissions(contract))
                return new HttpNotFoundResult("No permissions...");

            var changedbyUser = _userService.GetUser(contract.ChangedByUser_Id);
            var user = _userService.GetUser(SessionFacade.CurrentUser.Id);

            var contractEditInput = CreateInputViewModel(SessionFacade.CurrentCustomer.Id, user);

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

            return View(contractEditInput);
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
                var contract = _contractService.GetContract(id, SessionFacade.CurrentUser.Id);

                if (contract != null && HasPermissions(contract))
                  _contractService.DeleteContract(contract);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #region Helper Methods

        private bool HasPermissions(Contract contract)
        {
            var deps = _departmentService.GetDepartmentsForUser(SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentUser.Id, false);
            return !contract.Department_Id.HasValue || deps.Select(d => d.Id).Contains(contract.Department_Id.Value);
        }

        #endregion
    }
}
