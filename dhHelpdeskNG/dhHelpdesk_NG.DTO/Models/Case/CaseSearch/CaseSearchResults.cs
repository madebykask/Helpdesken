namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Domain;

    public class CaseSearchResult
    {
        public int Id { get; set; }
        public GlobalEnums.CaseIcon CaseIcon { get; set; }
        public string SortOrder { get; set; }
        public string SecSortOrder { get; set; }
        public string Tooltip { get; set; }
        public bool IsUnread { get; set; }
        public bool IsUrgent { get; set; }
        public IList<Field> Columns { get; set; }
        public bool Ignored { get; set; }
        public bool IsClosed { get; set; }

        public bool IsParent { get; set; }
        public int ParentId { get; set; }


        public ExtendedSearchInfo ExtendedSearchInfo { get; set; }
    }



    public class ExtendedSearchInfo
    {
        public int CustomerId { get; set; }
        public int WorkingGroupId { get; set; }
        public int DepartmentId { get; set; }

        public string ReportedBy { get; set; }
        public int? Performer_User_Id { get; set; }
        public int? CaseResponsibleUser_Id { get; set; }
        public int? User_Id { get; set; }

    }

    public class CaseAggregateData
    {
        public CaseAggregateData()
        {
            this.Status = new Dictionary<int, int>();
            this.SubStatus = new Dictionary<int, int>();
        }

        public IDictionary<int, int> Status { get; set; }

        public IDictionary<int, int> SubStatus { get; set; }
    }
}
