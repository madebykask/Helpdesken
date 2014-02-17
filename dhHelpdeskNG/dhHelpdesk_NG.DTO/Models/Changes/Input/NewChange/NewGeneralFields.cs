namespace DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewGeneralFields
    {
        public NewGeneralFields(
            int priority,
            string title,
            int? statusId,
            int? systemId,
            int? objectId,
            int? workingGroupId,
            int? administratorId,
            DateTime? finishingDate,
            DateTime createdDate,
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
            this.Rss = rss;
        }

        public int Priority { get; private set; }

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

        public bool Rss { get; private set; }
    }
}
