namespace DH.Helpdesk.BusinessData.Models.Projects.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class UpdatedProjectSchedule : INewBusinessModel
    {
        public UpdatedProjectSchedule(int id, int? userId, string name, int position, int state, int time, string description, DateTime? startDate, DateTime? finishDate, decimal? caseNumber, DateTime changeDate)
        {
            this.Id = id;
            this.UserId = userId;
            this.Name = name;
            this.Position = position;
            this.State = state;
            this.Time = time;
            this.Description = description;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.CaseNumber = caseNumber;
            this.ChangeDate = changeDate;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int? UserId { get; set; }

        [NotNullAndEmpty]
        public string Name { get; set; }

        [MinValue(0)]
        [MaxValue(99)]
        public int Position { get; set; }

        [MinValue(0)]
        [MaxValue(3)]
        public int State { get; set; }

        [MinValue(0)]
        public int Time { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public decimal? CaseNumber { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}