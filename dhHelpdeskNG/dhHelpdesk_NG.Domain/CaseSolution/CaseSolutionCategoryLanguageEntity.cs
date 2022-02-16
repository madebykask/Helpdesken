using System;
using global::System.Collections.Generic;
namespace DH.Helpdesk.Domain

{
    public class CaseSolutionCategoryLanguageEntity
    {
        public string CaseSolutionCategoryName { get; set; }

        public int Category_Id { get; set; }

        public int Language_Id { get; set; }

        public virtual CaseSolutionCategory CaseSolutionCategory { get; set; }
    }
}
