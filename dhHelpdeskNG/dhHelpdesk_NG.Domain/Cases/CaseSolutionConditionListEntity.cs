using System;

namespace DH.Helpdesk.Domain.Cases
{
    using global::System.Collections.Generic;


    public class CaseSolitionConditionListEntity : Entity
    {
        
        public Guid CaseSolutionConditionGUID { get; set; }

        public string CaseSolutionConditionValues { get; set; }
        
    }
}