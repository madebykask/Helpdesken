using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.FinishingCause;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.Web.Infrastructure.Helpers
{
    internal static class CommonHelper
    {
        public static string GetFinishingCauseFullPath(FinishingCauseInfo[] finishingCauses, int? finishingCauseId)
        {
            if (!finishingCauseId.HasValue)
            {
                return string.Empty;
            }

            var finishingCause = finishingCauses.FirstOrDefault(f => f.Id == finishingCauseId);
            if (finishingCause == null)
            {
                return string.Empty;
            }

            var list = new List<FinishingCauseInfo>();
            var parent = finishingCause;
            do
            {
                list.Add(parent);
                parent = finishingCauses.FirstOrDefault(c => c.Id == parent.ParentId);
            }
            while (parent != null);

            return string.Join(" - ", list.Select(c => c.Name).Reverse());
        }

        public static IList<CaseSearchResult> TreeTranslate(IList<CaseSearchResult> cases, int customerId, IProductAreaService productAreaService)
        {
            var ret = cases;
            var productareaCache = productAreaService.GetProductAreasForCustomer(customerId).ToDictionary(it => it.Id, it => true);
            foreach (CaseSearchResult r in ret)
            {
                foreach (var c in r.Columns)
                {
                    if (c.TreeTranslation)
                    {
                        switch (c.Key.ToLower())
                        {
                            case "productarea_id":
                                if (productareaCache.ContainsKey(c.Id))
                                {
                                    var names = productAreaService.GetParentPath(c.Id, customerId).Select(name => Translation.GetMasterDataTranslation(name));
                                    c.StringValue = string.Join(" - ", names);
                                }

                                break;
                        }
                    }
                }
            }

            return ret;
        }
    }
}