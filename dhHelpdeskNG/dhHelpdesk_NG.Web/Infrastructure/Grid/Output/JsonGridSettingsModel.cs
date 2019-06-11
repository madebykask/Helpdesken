namespace DH.Helpdesk.Web.Infrastructure.Grid.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Grid;

    /// <summary>
    /// This model intended for send grid column settings in JSON
    /// </summary>
    public class JsonGridColumnDef
    {
        public int id;

        public string field;

        public string displayName;

        public string cls;

        public bool isExpandable;

        // in percent 
        public string width;
    }

    /// <summary>
    /// This model intented to store grid settins to send/receive them in JSON
    /// </summary>
    public class JsonGridSettingsModel
    {
        public List<JsonGridColumnDef> columnDefs;

        public GridSortOptions sortOptions;

        public GridPageOptions pageOptions;

        public bool HasAvailableColumns;        

        public string cls;

	    public string[] pageSizeList;
    }
}
