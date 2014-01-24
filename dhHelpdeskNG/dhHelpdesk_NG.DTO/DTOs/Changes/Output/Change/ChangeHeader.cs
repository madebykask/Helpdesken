namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Change
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ChangeHeader
    {
        public ChangeHeader(
            string id,
            string name,
            string phone,
            string cellPhone,
            string email,
            int? departmentId,
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
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.DepartmentId = departmentId;
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

        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Phone { get; private set; }

        public string CellPhone { get; private set; }

        public string Email { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }

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
