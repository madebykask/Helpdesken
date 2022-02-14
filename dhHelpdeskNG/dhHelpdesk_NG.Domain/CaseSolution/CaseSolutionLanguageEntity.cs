using System;
using global::System.Collections.Generic;
namespace DH.Helpdesk.Domain

{
    public class CaseSolutionLanguageEntity
    {
        public string CaseSolutionName { get; set; }

        public string ShortDescription { get; set; }

        public string Information { get; set; }

        public int CaseSolution_Id { get; set; }

        public int Language_Id { get; set; }

        public virtual CaseSolution CaseSolution { get; set; }
    }
}
