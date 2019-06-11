using System;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Grid;
using DH.Helpdesk.Web.Infrastructure.Grid.Output;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.OldComponents;

namespace DH.Helpdesk.Web.Infrastructure.Grid
{
    public class JsonGridSettingsMapper
    {
        // expandable fields can be add to this array 
        private static readonly List<string> GridExpandableFields = new List<string>
        {
            GlobalEnums.TranslationCaseFields.Description.ToString()
        };

        public static JsonGridSettingsModel ToJsonGridSettingsModel(GridSettingsModel srcModel, int customerId, int availableColCount, string[] pageSizeList)
        {
            return new JsonGridSettingsModel
            {
                cls = srcModel.cls,
                pageOptions = srcModel.pageOptions,
                pageSizeList = pageSizeList,
                sortOptions = srcModel.sortOptions,
                HasAvailableColumns = availableColCount > 0,
                columnDefs = srcModel.columnDefs.Select(it => new JsonGridColumnDef()
                {
                    id = it.id,
                    cls = it.cls,
                    displayName = Translation.GetForCase(it.name, customerId),
                    field = it.name,
                    isExpandable = GridExpandableFields.Contains(it.name, StringComparer.OrdinalIgnoreCase),
                    width = it.width
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
            /* Keep 2% for icon */
            var fields = new Dictionary<string,string>()
            { 
                { GlobalEnums.TranslationCaseFields.CaseNumber.ToString(), "5%"}, 
                { GlobalEnums.TranslationCaseFields.Persons_Name.ToString(),"13%"},
                { GlobalEnums.TranslationCaseFields.Caption.ToString(),"20%"},
                { GlobalEnums.TranslationCaseFields.Description.ToString(),"25%"},
                { GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString(),"10%"},
                { GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString(),"10%"},
                { GlobalEnums.TranslationCaseFields.Department_Id.ToString(),"10%"},
                { GlobalEnums.TranslationCaseFields.RegTime.ToString(), "5%"}
            };

            var ret = fields.Select(field => new JsonGridColumnDef
            {
                id = GridColumnsDefinition.GetFieldId(field.Key),
                field = field.Key,
                displayName = Translation.GetForCase(field.Key, customerId),
                cls = "colnormal",
                isExpandable = GridExpandableFields.Contains(field.Key, StringComparer.OrdinalIgnoreCase),
                width = field.Value
            }).ToList();
            
            return ret;
        }
    }
}