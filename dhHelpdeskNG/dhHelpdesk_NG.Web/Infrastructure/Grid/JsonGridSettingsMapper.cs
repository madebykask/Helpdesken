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
                        GlobalEnums.TranslationCaseFields.CaseNumber.ToString(), 
                        GlobalEnums.TranslationCaseFields.Persons_Name.ToString(),
                        GlobalEnums.TranslationCaseFields.Caption.ToString(),
                        GlobalEnums.TranslationCaseFields.Description.ToString(),
                        GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString(),
                        GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString(),
                        GlobalEnums.TranslationCaseFields.Department_Id.ToString(),
                        GlobalEnums.TranslationCaseFields.RegTime.ToString() 
                    };

            // expandable fields can be add to this array 
            var expandableFields = new string[] 
                {
                    GlobalEnums.TranslationCaseFields.Description.ToString()
                };

            foreach (var fieldName in fieldNames)
            {
                var curCol = new JsonGridColumnDef 
                                    { 
                                        cls = "colnormal",
                                        displayName = Translation.Get(fieldName, Enums.TranslationSource.CaseTranslation, customerId),
                                        field = fieldName,
                                        isExpandable = expandableFields.Contains(fieldName)
                                    };            
                ret.Add(curCol);
            }

            return ret;
        }

       
    }
}