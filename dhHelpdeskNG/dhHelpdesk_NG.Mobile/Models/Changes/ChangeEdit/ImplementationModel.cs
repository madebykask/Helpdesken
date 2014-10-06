namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ImplementationModel
    {
        #region Constructors and Destructors

        public ImplementationModel()
        {
        }

        public ImplementationModel(
            int changeId,
            ConfigurableFieldModel<SelectList> implementationStatuses,
            ConfigurableFieldModel<DateTime?> realStartDate,
            ConfigurableFieldModel<DateTime?> finishingDate,
            ConfigurableFieldModel<bool> buildImplemented,
            ConfigurableFieldModel<bool> implementationPlanUsed,
            ConfigurableFieldModel<string> changeDeviation,
            ConfigurableFieldModel<bool> recoveryPlanUsed,
            ConfigurableFieldModel<AttachedFilesModel> attachedFiles,
            ConfigurableFieldModel<LogsModel> logs,
            ConfigurableFieldModel<bool> implementationReady)
        {
            this.ChangeId = changeId;
            this.ImplementationStatuses = implementationStatuses;
            this.RealStartDate = realStartDate;
            this.FinishingDate = finishingDate;
            this.BuildImplemented = buildImplemented;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.ChangeDeviation = changeDeviation;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.ImplementationReady = implementationReady;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public ConfigurableFieldModel<AttachedFilesModel> AttachedFiles { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> BuildImplemented { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> ChangeDeviation { get; set; }

        [IsId]
        public int ChangeId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> FinishingDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> ImplementationPlanUsed { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> ImplementationReady { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> ImplementationStatuses { get; set; }

        [NotNull]
        public ConfigurableFieldModel<LogsModel> Logs { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> RealStartDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> RecoveryPlanUsed { get; set; }

        [IsId]
        public int? ImplementationStatusId { get; set; }

        #endregion

        public bool HasShowableFields()
        {
            return this.AttachedFiles.Show ||
                this.BuildImplemented.Show ||
                this.ChangeDeviation.Show ||
                this.FinishingDate.Show ||
                this.ImplementationPlanUsed.Show ||
                this.ImplementationReady.Show ||
                this.ImplementationStatuses.Show ||
                this.Logs.Show ||
                this.RealStartDate.Show ||
                this.RecoveryPlanUsed.Show;
        }
    }
}