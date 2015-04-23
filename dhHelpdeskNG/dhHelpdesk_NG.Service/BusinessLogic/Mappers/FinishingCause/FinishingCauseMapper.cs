namespace DH.Helpdesk.Services.BusinessLogic.Mappers.FinishingCause
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.FinishingCause;

    public static class FinishingCauseMapper
    {
        public static List<FinishingCauseItem> BuildRelations(this IEnumerable<FinishingCauseItem> finishingCauses)
        {
            return finishingCauses.BuildLineRelations().Where(a => !a.ParentId.HasValue).ToList();
        }

        public static List<FinishingCauseItem> BuildLineRelations(this IEnumerable<FinishingCauseItem> finishingCauses)
        {
            foreach (var finishingCause in finishingCauses)
            {
                if (finishingCause.ParentId.HasValue)
                {
                    finishingCause.Parent = finishingCauses.FirstOrDefault(f => f.Id == finishingCause.ParentId);
                }

                finishingCause.Children.AddRange(finishingCauses.Where(f => f.ParentId == finishingCause.Id).OrderBy(f => f.Name));
            }

            return finishingCauses.OrderBy(f => f.Name).ToList();
        }
    }
}