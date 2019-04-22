using DH.Helpdesk.BusinessData.Models.Shared;

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

        public string PerformerUserName { get; set; }

        public string PriorityName { get; set; }

        public bool IsActive { get; set; }

        public string ConnectedToButton { get; set; }

        public int SortOrder { get; set; }

        public string CaseSolutionDescription { get; set; }

        public bool IsShowOnlyActive { get; set; }

        public string ImageClass { get; set; }
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
                                       PerformerUserName = userName,
                                       PriorityName =
                                           it.Priority == null ? string.Empty : it.Priority.Name,
                                       IsActive = (it.Status != 0),
                                       SortOrder = it.SortOrder
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


        public IList<Status> CaseSolutionStatuses { get; set; }

        public IList<WorkingGroupEntity> CaseSolutionWGroup { get; set; }

        public IList<Priority> CaseSolutionPriorities { get; set; }

        public IList<StateSecondary> CaseSolutionSubStatus { get; set; }

        public IList<ProductArea> CaseSolutionProductArea { get; set; }

        public IList<WorkingGroupEntity> CaseSolutionUserWGroup { get; set; }

        public IList<ProductArea> CaseSolutionCTemplateProductArea { get; set; }

        public IList<ItemOverview> CaseSolutionApplication { get; set; }
    }
}