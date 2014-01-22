namespace dhHelpdesk_NG.Web.Models.Changes.InputModel
{
    using System;
    using System.Web.Mvc;

    using DataAnnotationsExtensions;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class ChangeHeaderModel
    {
        public ChangeHeaderModel()
        {
        }

        public ChangeHeaderModel(
            string id,
            string name,
            string phone,
            string cellPhone,
            string email,
            SelectList department,
            string title,
            SelectList status,
            SelectList system,
            SelectList @object,
            SelectList workingGroup,
            SelectList administrator,
            DateTime? finishingDate,
            DateTime? createdDate,
            DateTime? changedDate,
            bool rss)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Department = department;
            this.Title = title;
            this.Status = status;
            this.System = system;
            this.Object = @object;
            this.WorkingGroup = workingGroup;
            this.Administrator = administrator;
            this.FinishingDate = finishingDate;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.Rss = rss;
        }

        [LocalizedDisplay("ID")]
        public string Id { get; set; }

        [LocalizedDisplay("Name")]
        public string Name { get; set; }

        [LocalizedDisplay("Phone")]
        public string Phone { get; set; }

        [LocalizedDisplay("Cell phone")]
        public string CellPhone { get; set; }

        [Email]
        [LocalizedDisplay("Email")]
        public string Email { get; set; }

        [LocalizedDisplay("Department")]
        public SelectList Department { get; set; }

        [IsId]
        public int? DepartmentId { get; set; }

        [LocalizedDisplay("Title")]
        public string Title { get; set; }

        [LocalizedDisplay("Status")]
        public SelectList Status { get; set; }

        [IsId]
        public int? StatusId { get; set; }

        [LocalizedDisplay("System")]
        public SelectList System { get; set; }

        [IsId]
        public int? SystemId { get; set; }

        [LocalizedDisplay("Object")]
        public SelectList Object { get; set; }

        [IsId]
        public int? ObjectId { get; set; }

        [LocalizedDisplay("Working group")]
        public SelectList WorkingGroup { get; set; }

        [IsId]
        public int? WorkingGroupId { get; set; }

        [LocalizedDisplay("Administrator")]
        public SelectList Administrator { get; set; }

        [IsId]
        public int? AdministratorId { get; set; }

        [LocalizedDisplay("Finishing date")]
        public DateTime? FinishingDate { get; set; }

        [LocalizedDisplay("Created date")]
        public DateTime? CreatedDate { get; set; }

        [LocalizedDisplay("Changed date")]
        public DateTime? ChangedDate { get; set; }

        [LocalizedDisplay("Rss")]
        public bool Rss { get; set; }
    }
}