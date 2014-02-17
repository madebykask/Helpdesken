namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class GeneralModel
    {
        #region Constructors and Destructors

        public GeneralModel()
        {
        }

        public GeneralModel(
            ConfigurableFieldModel<int> priority,
            ConfigurableFieldModel<string> title,
            ConfigurableFieldModel<SelectList> status,
            ConfigurableFieldModel<SelectList> system,
            ConfigurableFieldModel<SelectList> @object,
            ConfigurableFieldModel<SelectList> workingGroup,
            ConfigurableFieldModel<SelectList> administrator,
            ConfigurableFieldModel<DateTime?> finishingDate,
            DateTime? createdDate,
            DateTime? changedDate,
            ConfigurableFieldModel<bool> rss)
        {
            this.Priority = priority;
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

        #endregion

        #region Public Properties

        [NotNull]
        public ConfigurableFieldModel<int> Priority { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Title { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Status { get; private set; }

        [IsId]
        public int? StatusId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> System { get; private set; }

        [IsId]
        public int? SystemId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Object { get; private set; }

        [IsId]
        public int? ObjectId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> WorkingGroup { get; private set; }

        [IsId]
        public int? WorkingGroupId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Administrator { get; private set; }

        [IsId]
        public int? AdministratorId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> FinishingDate { get; set; }

        [LocalizedDisplay("Created Date")]
        public DateTime? CreatedDate { get; set; }

        [LocalizedDisplay("Changed Date")]
        public DateTime? ChangedDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> Rss { get; set; }

        #endregion
    }
}