namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.LocalizedAttributes;

    public sealed class GeneralModel
    {
        #region Constructors and Destructors

        public GeneralModel()
        {
        }

        public GeneralModel(
            ConfigurableFieldModel<int> priority,
            ConfigurableFieldModel<string> title,
            ConfigurableFieldModel<DateTime?> finishingDate,
            DateTime? createdDate,
            DateTime? changedDate,
            ConfigurableFieldModel<bool> rss)
        {
            this.Priority = priority;
            this.Title = title;
            this.FinishingDate = finishingDate;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.Rss = rss;
        }

        #endregion

        #region Public Properties

        [IsId]
        public int? AdministratorId { get; set; }

        [LocalizedDisplay("Changed Date")]
        public DateTime? ChangedDate { get; set; }

        [LocalizedDisplay("Created Date")]
        public DateTime? CreatedDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> FinishingDate { get; set; }

        [IsId]
        public int? ObjectId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int> Priority { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> Rss { get; set; }

        [IsId]
        public int? StatusId { get; set; }

        [IsId]
        public int? SystemId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Title { get; set; }

        [IsId]
        public int? WorkingGroupId { get; set; }

        #endregion
    }
}