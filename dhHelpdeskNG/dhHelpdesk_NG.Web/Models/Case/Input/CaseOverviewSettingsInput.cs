namespace DH.Helpdesk.Web.Models.Case.Input
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Grid;

    public class ColumnSettings
    {
        public string ColumnName { get; set; }

        public string Style { get; set; }
    }

    public class CaseOverviewSettingsInput
    {
        public string SelectedFontStyle { get; set; }

        public ICollection<ColumnSettings> FieldStyle { get; set; }

        internal GridSettingsModel MapToGridSettingsModel()
        {
            var res = new GridSettingsModel
                          {
                              cls = this.SelectedFontStyle,
                              columnDefs =
                                  this.FieldStyle.Select(
                                      it =>
                                      new GridColumnDef
                                          {
                                              id = GridColumnsDefinition.GetFieldId(it.ColumnName),
                                              name = it.ColumnName,
                                              cls = it.Style
                                          }).ToList()
                          };
            return res;
        }
    }
}