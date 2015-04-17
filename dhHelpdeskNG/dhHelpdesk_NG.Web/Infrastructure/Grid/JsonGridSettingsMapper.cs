namespace DH.Helpdesk.Web.Infrastructure.Grid
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.Web.Infrastructure.Grid.Output;

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
                columnDefs = srcModel.columnDefs.Select(it => new JsonGridColumnDef() { cls = it.cls, displayName = Translation.Get(it.name, Enums.TranslationSource.CaseTranslation, customerId), field = it.name}).ToList()
            };
        }
    }
}