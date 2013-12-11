using System;

namespace dhHelpdesk_NG.Domain
{
    public class ProjectSchedule : Entity
    {
        public ProjectSchedule()
        {
            IsActive = 1;
        }

        public double CaseNumber { get; set; }
        public int CalculatedTime { get; set; }
        public int IsActive { get; set; }
        public int Parent_ProjectSchedule_Id { get; set; }
        public int Pos { get; set; }
        public int Project_Id { get; set; }
        public int TimeType_Id { get; set; }
        public int User_Id { get; set; }
        public string Activity { get; set; }
        public string CalculatedDate { get; set; }
        public string Note { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime ScheduleDate { get; set; }

        public virtual Project Project { get; set; }
        public virtual TimeType TimeType { get; set; }
        public virtual User User { get; set; }
    }
}
