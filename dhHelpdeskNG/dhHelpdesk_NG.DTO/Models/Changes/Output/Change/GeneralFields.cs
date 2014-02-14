namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Change
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GeneralFields
    {
        public GeneralFields(
            int? priority,
            string title,
            int? statusId,
            int? systemId,
            int? objectId,
            int? workingGroupId,
            int? administratorId,
            DateTime? finishingDate,
            DateTime createdDate,
            DateTime? changedDate,
            bool rss)
        {
            this.Priority = priority;
            this.Title = title;
            this.StatusId = statusId;
            this.SystemId = systemId;
            this.ObjectId = objectId;
            this.WorkingGroupId = workingGroupId;
            this.AdministratorId = administratorId;
            this.FinishingDate = finishingDate;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.Rss = rss;
        }

        public int? Priority { get; private set; }

        public string Title { get; private set; }

        [IsId]
        public int? StatusId { get; private set; }

        [IsId]
        public int? SystemId { get; private set; }

        [IsId]
        public int? ObjectId { get; private set; }

        [IsId]
        public int? WorkingGroupId { get; private set; }

        [IsId]
        public int? AdministratorId { get; private set; }

        public DateTime? FinishingDate { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime? ChangedDate { get; private set; }

        public bool Rss { get; private set; }
    }
}
