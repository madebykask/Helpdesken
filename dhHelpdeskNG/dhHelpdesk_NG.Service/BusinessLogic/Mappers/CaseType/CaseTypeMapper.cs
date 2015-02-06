namespace DH.Helpdesk.Services.BusinessLogic.Mappers.CaseType
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.CaseType;

    public static class CaseTypeMapper
    {
        public static List<CaseTypeItem> BuildRelations(this IEnumerable<CaseTypeItem> caseTypes)
        {
            return caseTypes.BuildLineRelations().Where(a => !a.ParentId.HasValue).ToList();
        }

        public static List<CaseTypeItem> BuildLineRelations(this IEnumerable<CaseTypeItem> caseTypes)
        {
            foreach (var caseType in caseTypes)
            {
                if (caseType.ParentId.HasValue)
                {
                    caseType.Parent = caseTypes.FirstOrDefault(a => a.Id == caseType.ParentId);
                }

                caseType.Children.AddRange(caseTypes.Where(a => a.ParentId == caseType.Id));
            }

            return caseTypes.ToList();
        }
    }
}