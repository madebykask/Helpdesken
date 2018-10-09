namespace DH.Helpdesk.Web.Infrastructure.Grid
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.Web.Infrastructure.Grid.Output;
    using System.Collections.Generic;
    using DH.Helpdesk.BusinessData.OldComponents;

    public class JsonGridSettingsMapper
    {
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
                    cls = it.cls,
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

            // expandable fields can be add to this array 
            var expandableFields = new string[] 
                {
                    GlobalEnums.TranslationCaseFields.Description.ToString()                    
                };

            foreach (var field in fields)
            {
                var curCol = new JsonGridColumnDef 
                                    { 
                                        cls = "colnormal",
                                        displayName = Translation.Get(field.Key, Enums.TranslationSource.CaseTranslation, customerId),
                                        field = field.Key,
                                        isExpandable = expandableFields.Contains(field.Key),
                                        width = field.Value
                                    };            
                ret.Add(curCol);
            }

            return ret;
        }

       
    }
}