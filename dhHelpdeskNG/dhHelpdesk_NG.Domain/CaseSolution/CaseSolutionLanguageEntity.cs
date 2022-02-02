using System;
namespace DH.Helpdesk.Domain

{
    public class CaseSolutionLanguageEntity: EntityBase
    {
        public string CaseSolutionName { get; set; }

        public string ShortDescription { get; set; }

        public string Information { get; set; }

        public int CaseSolution_Id { get; set; }

        public int Language_Id { get; set; }

        public virtual CaseSolution CaseSolution { get; set; }
    }
}
