namespace DH.Helpdesk.Web.Infrastructure.Grid
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.Web.Infrastructure.Grid.Output;
    using System.Collections.Generic;
    using DH.Helpdesk.BusinessData.OldComponents;

    public class JsonGridSettingsMapper
    {
        public static JsonGridSettingsModel ToJsonGridSettingsModel(GridSettingsModel srcModel, int customerId, int availableColCount)
        {
            return new JsonGridSettingsModel
            {
                cls = srcModel.cls,
                pageOptions = srcModel.pageOptions,
                sortOptions = srcModel.sortOptions,
                HasAvailableColumns = availableColCount > 0,
                columnDefs = srcModel.columnDefs.Select(it => new JsonGridColumnDef() 
                                    { cls = it.cls, 
                                      displayName = Translation.Get(it.name, Enums.TranslationSource.CaseTranslation, customerId), 
                                      field = it.name
                                    }).ToList()
            };
        }


        public static JsonGridSettingsModel GetAdvancedSearchGridSettingsModel(int customerId)
        {
            return new JsonGridSettingsModel
            {
                cls = "normaltext",
                pageOptions = new GridPageOptions { pageIndex = 0, recPerPage = 0 },
                sortOptions = new GridSortOptions { sortBy = string.Empty, sortDir = SortingDirection.Desc },
                HasAvailableColumns = true,
                columnDefs = GetAdvancedSearchGridCols(customerId)
                
            };
        }

        private static List<JsonGridColumnDef> GetAdvancedSearchGridCols(int customerId)
        {
            var ret = new List<JsonGridColumnDef>();            
            string[] fieldNames = new string[] 
                    { 
                        "CaseNumber", "ReportedBy", "Caption", "Description",
                        "Performer_User_Id", "WorkingGroup_Id", "Department_Id", "RegTime" 
                    };
                        
            foreach (var fieldName in fieldNames)
            {
                var curCol = new JsonGridColumnDef 
                                    { 
                                        cls = "colnormal",
                                        displayName = Translation.Get(fieldName, Enums.TranslationSource.CaseTranslation, customerId),
                                        field = fieldName
                                    };            
                ret.Add(curCol);
            }

            return ret;
        }

       
    }
}