namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class HistoryItem
    {
        public HistoryItem(
            int id,
            DateTime dateAndTime,
            string registeredBy,
            int? administratorId,
            string administrator,
            int? objectId,
            string @object,
            int? priorityId,
            string priority,
            int? workingGroupId,
            string workingGroup,
            int? systemId,
            string system,
            int? categoryId,
            string category,
            int? statusId,
            string status)
        {
            this.Id = id;
            this.DateAndTime = dateAndTime;
            this.RegisteredBy = registeredBy;
            this.AdministratorId = administratorId;
            this.Administrator = administrator;
            this.ObjectId = objectId;
            this.Object = @object;
            this.PriorityId = priorityId;
            this.Priority = priority;
            this.WorkingGroupId = workingGroupId;
            this.WorkingGroup = workingGroup;
            this.SystemId = systemId;
            this.System = system;
            this.CategoryId = categoryId;
            this.Category = category;
            this.StatusId = statusId;
            this.Status = status;
        }

        [IsId]
        public int Id { get; private set; }

        public DateTime DateAndTime { get; private set; }

        public string RegisteredBy { get; private set; }

        [IsId]
        public int? AdministratorId { get; private set; }

        public string Administrator { get; private set; }

        [IsId]
        public int? ObjectId { get; private set; }

        public string Object { get; private set; }

        [IsId]
        public int? PriorityId { get; private set; }

        public string Priority { get; private set; }

        [IsId]
        public int? WorkingGroupId { get; private set; }

        public string WorkingGroup { get; private set; }

        [IsId]
        public int? SystemId { get; private set; }

        public string System { get; private set; }

        [IsId]
        public int? CategoryId { get; private set; }

        public string Category { get; private set; }

        [IsId]
        public int? StatusId { get; private set; }

        public string Status { get; private set; }
    }
}
