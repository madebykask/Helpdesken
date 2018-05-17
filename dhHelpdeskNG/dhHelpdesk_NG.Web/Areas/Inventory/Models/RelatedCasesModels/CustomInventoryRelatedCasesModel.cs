using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Inventory.Models.RelatedCasesModels
{
    public class CustomInventoryRelatedCasesModel : BaseEditCustomInventoryModel
    {
        public CustomInventoryRelatedCasesModel(int id) : base(id)
        {
            Id = id;
        }

        public RelatedCasesModel RelatedCases { get; set; }

        public override CustomInventoryTabs Tab { get { return CustomInventoryTabs.Cases; } }
    }
}