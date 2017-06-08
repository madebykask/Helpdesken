using System;

namespace DH.Helpdesk.Domain
{
    public class CaseSolutionConditionEntity : Entity
    {
        public int CaseSolution_Id { get; set; }
        public Guid CaseSolutionConditionGUID { get; set; }
        public string Property_Name { get; set; }
        public string Values { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUser_Id { get; set; }

        public DateTime ChangedDate { get; set; }
        public int? ChangedByUser_Id { get; set; }
        
    }
}
