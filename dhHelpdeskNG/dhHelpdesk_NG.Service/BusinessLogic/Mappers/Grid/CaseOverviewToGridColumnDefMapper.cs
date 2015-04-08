namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Grid
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.Domain.Grid;

    public static class CaseOverviewToGridColumnDefMapper
    {
        /// <summary>
        /// Maps data from GridSettingsEntity and caseOverviewSettings to GridColumnDef
        /// </summary>
        /// <param name="columnSettions"></param>
        /// <param name="caseOverviewSettings"></param>
        /// <returns></returns>
        public static List<GridColumnDef> MapToColumnDefinitions(this IQueryable<GridSettingsEntity> columnSettions, CaseOverviewGridColumnSetting[] caseOverviewSettings)
        {
            return
                caseOverviewSettings.Select(
                    it => new GridColumnDef() { id = GridColumnsDefinition.GetFieldId(it.Name), name = it.Name, cls = it.Style }).ToList();
        }

        public static CaseOverviewGridColumnSetting[] MapToCaseOverviewSettings(
            this IEnumerable<GridColumnDef> colunDefs)
        {
            var orderIdx = 0;
            return colunDefs.Select(
                    it => new CaseOverviewGridColumnSetting { Name = it.name, Order = orderIdx++, Style = it.cls })
                    .ToArray();
        }
    }
}
