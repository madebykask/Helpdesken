using System.ComponentModel.DataAnnotations;

namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Projects;

    using global::System;

    public class CaseSolution : Entity
    {
        public int? CaseSolutionCategory_Id { get; set; }
        public int? CaseWorkingGroup_Id { get; set; }
        public int? CaseType_Id { get; set; }
        public int? Category_Id { get; set; }
        public int? Department_Id { get; set; }
        public int Customer_Id { get; set; }
        public int? FinishingCause_Id { get; set; }
        public int NoMailToNotifier { get; set; }
        public int? PerformerUser_Id { get; set; }
        public int? Priority_Id { get; set; }
        public int? ProductArea_Id { get; set; }
        public int? Project_Id { get; set; }
        public int? WorkingGroup_Id { get; set; }        
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Miscellaneous { get; set; }
        [Required]
        public string Name { get; set; }
        public string ReportedBy { get; set; }
        public string Text_External { get; set; }
        public string Text_Internal { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string TemplatePath { get; set; }
        public bool ShowInSelfService { get; set; }

        public virtual CaseSolutionCategory CaseSolutionCategory { get; set; }
        public virtual CaseSolutionSchedule CaseSolutionSchedule { get; set; }
        public virtual CaseType CaseType { get; set; }
        public virtual Category Category { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual FinishingCause FinishingCause { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual ProductArea ProductArea { get; set; }
        public virtual Project Project { get; set; }
        public virtual User PerformerUser { get; set; }
        public virtual WorkingGroupEntity CaseWorkingGroup { get; set; }
        public virtual WorkingGroupEntity WorkingGroup { get; set; }
    }
}
