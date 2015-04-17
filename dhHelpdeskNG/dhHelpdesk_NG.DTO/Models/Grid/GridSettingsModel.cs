namespace DH.Helpdesk.BusinessData.Models.Grid
{
    using System.Collections.Generic;

    public enum SortingDirection
    {
        Asc,
        Desc
    }

    public class GridSortOptions
    {
        private const string AscStr = "asc";
        private const string DescStr = "desc";

        /// <summary>
        /// Name of the field wich we use to sort data. Should point to GridColumnDef.Name
        /// </summary>
        public string sortBy;

        public SortingDirection sortDir;

        public static string SortDirectionAsString(SortingDirection sortDir)
        {
            return sortDir == SortingDirection.Asc ? AscStr : DescStr;
        }

        public static SortingDirection SortDirectionFromString(string sortDirStr)
        {
            return sortDirStr == AscStr ? SortingDirection.Asc : SortingDirection.Desc;
        }
    }

    public class GridPageOptions
    {
        public int pageIndex;

        public int recPerPage;
    }

    public class GridColumnDef
    {
        public int id;

        public string name;       

        public string cls;
    }

    /// <summary>
    /// This class holds users settings for each grid for each customer
    /// </summary>
    public class GridSettingsModel
    {
        public List<GridColumnDef> columnDefs;

        public GridSortOptions sortOptions;

        public GridPageOptions pageOptions;

        public string cls;
    }
}