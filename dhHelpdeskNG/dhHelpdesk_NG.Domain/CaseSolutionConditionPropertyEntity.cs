using System;

namespace DH.Helpdesk.Domain
{
    public class CaseSolutionConditionPropertyEntity : Entity
    {
        public int Id { get; set; }
        public string CaseSolutionConditionProperty { get; set; }
        public string Text { get; set; }
        
    }
}
