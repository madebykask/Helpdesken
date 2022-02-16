namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class CaseSolutionCategory : Entity
    {
        public int Customer_Id { get; set; }
        public int IsDefault { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<CaseSolutionCategoryLanguageEntity> CaseSolutionCategoryLanguages { get; set; }
    }

}
