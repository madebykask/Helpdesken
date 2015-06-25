namespace DH.Helpdesk.Web.Models.CaseSolution
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Domain;

    public class RowIndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public string CaseCaption { get; set; }

        public string PerormerUserName { get; set; }

        public string PriorityName { get; set; }
    }

    public static class CaseSolutionRowIndexMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceData"></param>
        /// <param name="isUserFirstLastNameRepresentation"></param>
        /// <returns></returns>
        public static RowIndexViewModel[] MapToRowIndexViewModel(this IList<CaseSolution> sourceData, bool isUserFirstLastNameRepresentation = false)
        {
            return sourceData.OrderBy(it => it.Name)
                .Select(it =>
                    {
                        var userName = it.PerformerUser == null
                                           ? string.Empty
                                           : isUserFirstLastNameRepresentation
                                                 ? string.Format(
                                                     "{0} {1}",
                                                     it.PerformerUser.FirstName,
                                                     it.PerformerUser.SurName)
                                                 : string.Format(
                                                     "{0} {1}",
                                                     it.PerformerUser.SurName,
                                                     it.PerformerUser.FirstName);
                        return new RowIndexViewModel
                                   {
                                       Id = it.Id,
                                       Name = it.Name,
                                       CategoryName =
                                           it.CaseSolutionCategory == null
                                               ? string.Empty
                                               : it.CaseSolutionCategory.Name,
                                       CaseCaption = it.Caption,
                                       PerormerUserName = userName,
                                       PriorityName =
                                           it.Priority == null ? string.Empty : it.Priority.Name
                                   };
                    }).ToArray();
        }
    }

    public class CaseSolutionIndexViewModel
    {
        public CaseSolutionIndexViewModel(string activeTab)
        {
            this.ActiveTab = activeTab;
        }

        public string SearchCss { get; set; }

        public CaseSolution CaseSolution { get; set; }

        public IEnumerable<CaseSolution> CSolutions { get; set; }

        public RowIndexViewModel[] Rows { get; set; }

        public IList<CaseSolutionCategory> CaseSolutionCategories { get; set; }

        public string ActiveTab { get; set; }
    }
}