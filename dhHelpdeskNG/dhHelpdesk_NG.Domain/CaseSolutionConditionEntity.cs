using System;

namespace DH.Helpdesk.Domain
{
    public class CaseSolutionConditionEntity : Entity
    {
        //public int Id { get; set; }
        public int CaseSolution_Id { get; set; }
        public Guid CaseSolutionConditionGUID { get; set; }
        public string CaseField_Name { get; set; }
        public string Values { get; set; }
        public int Sequence { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ChangedDate { get; set; }
    }
}
