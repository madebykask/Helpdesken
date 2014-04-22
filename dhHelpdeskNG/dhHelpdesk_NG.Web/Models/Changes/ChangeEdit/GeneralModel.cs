namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class GeneralModel
    {
        #region Constructors and Destructors

        public GeneralModel()
        {
        }

        public GeneralModel(
            bool isNew,
            ConfigurableFieldModel<int> prioritisation,
            ConfigurableFieldModel<string> title,
            ConfigurableFieldModel<SelectList> statuses,
            ConfigurableFieldModel<SelectList> systems,
            ConfigurableFieldModel<SelectList> objects,
            ConfigurableFieldModel<SelectList> workingGroups,
            ConfigurableFieldModel<SelectList> administrators,
            ConfigurableFieldModel<DateTime?> finishingDate,
            DateTime? createdDate,
            DateTime? changedDate,
            UserName changedByUser,
            ConfigurableFieldModel<bool> rss)
        {
            this.IsNew = isNew;
            this.Prioritisation = prioritisation;
            this.Title = title;
            this.Statuses = statuses;
            this.Systems = systems;
            this.Objects = objects;
            this.WorkingGroups = workingGroups;
            this.Administrators = administrators;
            this.FinishingDate = finishingDate;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.ChangedByUser = changedByUser;
            this.Rss = rss;
        }

        #endregion

        #region Public Properties

        public bool IsNew { get; set; }

        [IsId]
        public int? AdministratorId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Administrators { get; set; }

        public UserName ChangedByUser { get; set; }

        [LocalizedDisplay("Changed Date")]
        public DateTime? ChangedDate { get; set; }

        [LocalizedDisplay("Created Date")]
        public DateTime? CreatedDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> FinishingDate { get; set; }

        [IsId]
        public int? ObjectId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Objects { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int> Prioritisation { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> Rss { get; set; }

        [IsId]
        public int? StatusId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Statuses { get; set; }

        [IsId]
        public int? SystemId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Systems { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Title { get; set; }

        [IsId]
        public int? WorkingGroupId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> WorkingGroups { get; set; }

        #endregion
    }
}