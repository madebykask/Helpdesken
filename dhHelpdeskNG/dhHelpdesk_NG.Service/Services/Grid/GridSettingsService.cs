namespace DH.Helpdesk.Services.Services.Grid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain.Grid;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Grid;

    /// <summary>
    /// Handles all operations with grid settings
    /// </summary>
    public class GridSettingsService
    {
        public const int CASE_OVERVIEW_GRID_ID = 1;
        public const int CASE_CONNECTPARENT_GRID_ID = 2;
        public const int CASE_CONTRACT_CASES_GRID = 3;

        #region Constants for default values

        public const string DEFAULT_GRID_CLS = "normal";

        public const int DEFAULT_REC_PER_PAGE = 50;
        public const int DEFAULT_REC_PER_PAGE_FOR_CONNECTPARENT = 5;

        #endregion

        #region Keys

        private const string GRID_CLS_KEY = "font_style";

        private const string RECORDS_PER_PAGE_KEY = "recs_per_page";

        private const string SORT_BY_KEY = "grid_sort_by";

        private const string SORT_DIR_KEY = "grid_sort_dir";

        #endregion

        #region Fields

        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        
        private readonly CaseSettingsService caseSettingsService;

        #endregion

        #region Constructors and Destructors

        public GridSettingsService(IUnitOfWorkFactory unitOfWorkFactory, CaseSettingsService caseSettingsService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.caseSettingsService = caseSettingsService;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns grid settings business model
        /// @TODO (alexander.semenischev): implemented only for caseoverview talbe, need to implement for "standart"/other grids
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="userGroupId"></param>
        /// <param name="userId"></param>
        /// <param name="gridId"></param>
        /// <returns></returns>
        public GridSettingsModel GetForCustomerUserGrid(int customerId, int userGroupId, int userId, int gridId)
        {
            var res = new GridSettingsModel();
            
            switch (gridId)
            {
                case CASE_OVERVIEW_GRID_ID:
                    using (IUnitOfWork uow = this.unitOfWorkFactory.Create())
                    {
                        var repository = uow.GetRepository<GridSettingsEntity>();
                        var allGridParams =
                            repository.GetAll()
                                .Where(item => item.CustomerId == customerId && item.UserId == userId && item.GridId == gridId)
                                .AsQueryable();
                        var gridParams = allGridParams.Where(it => !it.FieldId.HasValue)
                            .ToDictionary(it => it.Parameter.Trim(), it => it.Value.Trim());

                        res.CustomerId = customerId;
                        res.cls = gridParams.ContainsKey(GRID_CLS_KEY) ? gridParams[GRID_CLS_KEY] : DEFAULT_GRID_CLS;
                        res.sortOptions = MapSortOptions(gridParams);
                        res.pageOptions = MapPageOptions(gridParams);
                        var columnSettings = this.caseSettingsService.GetSelectedCaseOverviewGridColumnSettings(customerId, userId);
                        var columns = columnSettings as CaseOverviewGridColumnSetting[] ?? columnSettings.ToArray();

                        res.columnDefs = allGridParams.Where(it => it.FieldId.HasValue).MapToColumnDefinitions(columns);
                    }
                    if (!res.columnDefs.Any())
                    {
                        res.columnDefs.AddRange(this.GetDefaultColumns(customerId, userGroupId, gridId));
                    }

                    res.sortOptions.sortBy = string.Empty;

                    return res;
                case CASE_CONNECTPARENT_GRID_ID:
                case CASE_CONTRACT_CASES_GRID:
                    using (IUnitOfWork uow = this.unitOfWorkFactory.Create())
                    {
                        var repository = uow.GetRepository<GridSettingsEntity>();
                        var allGridParams =
                            repository.GetAll()
                                .Where(item => item.CustomerId == customerId && item.UserId == userId && item.GridId == CASE_OVERVIEW_GRID_ID)
                                .AsQueryable();
                        var gridParams = allGridParams.Where(it => !it.FieldId.HasValue)
                            .ToDictionary(it => it.Parameter.Trim(), it => it.Value.Trim());

                        res.CustomerId = customerId;
                        res.cls = gridParams.ContainsKey(GRID_CLS_KEY) ? gridParams[GRID_CLS_KEY] : DEFAULT_GRID_CLS;
                        res.sortOptions = MapSortOptions(gridParams);
                    }
                    res.columnDefs = new List<GridColumnDef>();
                    res.columnDefs.AddRange(this.GetDefaultColumns(customerId, userGroupId, gridId));
                    res.pageOptions = new GridPageOptions
                    {
                        pageIndex = 0,
                        recPerPage = DEFAULT_REC_PER_PAGE_FOR_CONNECTPARENT
                    };
                    return res;
            }
            return res;
        }

        /// <summary>
        /// Returns all available for current customer columns for grid
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="userGroupId"></param>
        /// <param name="gridId"></param>
        /// <returns></returns>
        public List<GridColumnDef> GetDefaultColumns(int customerId, int userGroupId, int gridId)
        {
            switch (gridId)
            {
                case CASE_OVERVIEW_GRID_ID:
                    var duplicates = new HashSet<string>();
                    return
                        this.caseSettingsService.GetAvailableCaseOverviewGridColumnSettingsByUserGroup(customerId, userGroupId)
                        .Where(it => !duplicates.Contains(it.Name.ToLower()))
                            .Select(
                                it =>
                                    {
                                        duplicates.Add(it.Name.ToLower());
                                        return new GridColumnDef
                                                   {
                                                       id = GridColumnsDefinition.GetFieldId(it.Name),
                                                       name = it.Name,
                                                       cls = string.Empty
                                                   };
                                    })
                            .ToList();
                case CASE_CONNECTPARENT_GRID_ID:
                case CASE_CONTRACT_CASES_GRID:
                    return
                        this.caseSettingsService.GetConnectToParentGridColumnSettings(customerId, userGroupId)
                            .Select(it => new GridColumnDef
                            {
                                id = GridColumnsDefinition.GetFieldId(it.Name),
                                name = it.Name,
                                cls = string.Empty
                            })
                            .ToList();
                default:
                    throw new NotImplementedException(string.Format("GetAvailableColumnNames() is not implmented for supplyed grid_ID {0}", gridId));
            }
        }

        /// <summary>
        /// Saves settings in DB for "Case overview" grid
        /// </summary>
        /// <param name="inputModel"></param>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <param name="userGroupId"></param>
        public void SaveCaseoviewSettings(GridSettingsModel inputModel, int customerId, int userId, int userGroupId)
        {
            var oldModel = this.GetForCustomerUserGrid(customerId, userGroupId, userId, CASE_OVERVIEW_GRID_ID);

            //// in case when we have "Case overview" grid we should save settings into different talbes
            using (IUnitOfWork uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<GridSettingsEntity>();
                repository.DeleteWhere(it => it.CustomerId == customerId && it.UserId == userId && it.GridId == CASE_OVERVIEW_GRID_ID && it.Parameter == GRID_CLS_KEY);
                /// add class of the grid
                repository.Add(new GridSettingsEntity
                                        {
                                            CustomerId = customerId,
                                            UserId = userId,
                                            GridId = CASE_OVERVIEW_GRID_ID,
                                            Parameter = GRID_CLS_KEY,
                                            Value = inputModel.cls
                                        });
                /// records per page
                if (inputModel.pageOptions != null)
                {
                    repository.DeleteWhere(it => it.CustomerId == customerId && it.UserId == userId && it.GridId == CASE_OVERVIEW_GRID_ID && it.Parameter == RECORDS_PER_PAGE_KEY);
                    repository.Add(
                        new GridSettingsEntity
                            {
                                CustomerId = customerId,
                                UserId = userId,
                                GridId = CASE_OVERVIEW_GRID_ID,
                                Parameter = RECORDS_PER_PAGE_KEY,
                                Value = inputModel.pageOptions.recPerPage.ToString()
                            });
                }
                
                //// sortBy and sortOrder
                if (inputModel.sortOptions != null || !IsSortFieldAvailable(oldModel.sortOptions.sortBy, inputModel.columnDefs))
                {
                    repository.DeleteWhere(it => it.CustomerId == customerId 
                        && it.UserId == userId 
                        && it.GridId == CASE_OVERVIEW_GRID_ID
                        && (it.Parameter == SORT_BY_KEY || it.Parameter == SORT_DIR_KEY));
                }

                if (inputModel.sortOptions != null)
                {
                    repository.Add(
                        new GridSettingsEntity
                            {
                                CustomerId = customerId,
                                UserId = userId,
                                GridId = CASE_OVERVIEW_GRID_ID,
                                Parameter = SORT_BY_KEY,
                                Value = inputModel.sortOptions.sortBy
                            });
                    repository.Add(
                        new GridSettingsEntity
                        {
                            CustomerId = customerId,
                            UserId = userId,
                            GridId = CASE_OVERVIEW_GRID_ID,
                            Parameter = SORT_DIR_KEY,
                            Value = GridSortOptions.SortDirectionAsString(inputModel.sortOptions.sortDir)
                        });
                }

                uow.Save();
            }

            this.caseSettingsService.SyncSettings(inputModel.columnDefs.MapToCaseOverviewSettings(), customerId, userId, userGroupId);
        }

        private static bool IsSortFieldAvailable(string sortBy, IEnumerable<GridColumnDef> columnDefs)
        {
            return !string.IsNullOrEmpty(sortBy) && columnDefs.Any(column => column.name == sortBy);
        }

        private static GridSortOptions MapSortOptions(Dictionary<string, string> gridParams)
        {
            return new GridSortOptions
                          {
                              sortBy =
                                  gridParams.ContainsKey(SORT_BY_KEY)
                                      ? gridParams[SORT_BY_KEY]
                                      : string.Empty,
                              sortDir = gridParams.ContainsKey(SORT_DIR_KEY) ? GridSortOptions.SortDirectionFromString(gridParams[SORT_DIR_KEY]): SortingDirection.Desc
                          };
        }

        private static GridPageOptions MapPageOptions(Dictionary<string, string> gridParams)
        {
            return new GridPageOptions
                       {
                           pageIndex = 0,
                           recPerPage =
                               gridParams.ContainsKey(RECORDS_PER_PAGE_KEY)
                                   ? int.Parse(gridParams[RECORDS_PER_PAGE_KEY])
                                   : DEFAULT_REC_PER_PAGE
                       };
        }
        #endregion
    }
}