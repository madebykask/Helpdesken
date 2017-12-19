using System;

namespace DH.Helpdesk.Domain
{
    public class CaseSolutionConditionEntity : EntityBase
    {
        public int CaseSolution_Id { get; set; }
        public Guid CaseSolutionConditionGUID { get; set; }
        public string Property_Name { get; set; }
        public string Values { get; set; }
    }
}
