using DH.Helpdesk.BusinessData.Enums.Inventory;
using DH.Helpdesk.Web.Models.Case;

namespace DH.Helpdesk.Web.Areas.Inventory.Models
{
    public class RelatedCasesModel
    {
        public int InventoryId { get; set; }

        public CaseSearchResultModel SearchResult { get; set; }

        public string SortBy { get; set; }

        public bool SortByAsc { get; set; }

        public CurrentModes InventoryType { get; set; }
    }
}