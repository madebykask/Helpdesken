namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedImplementationFieldSettingGroupDto
    {
        public UpdatedImplementationFieldSettingGroupDto(
            UpdatedFieldSettingDto state,
            UpdatedFieldSettingDto realStartDate,
            UpdatedFieldSettingDto buildAndTextImplemented,
            UpdatedFieldSettingDto implementationPlanUsed,
            UpdatedStringFieldSettingDto deviation,
            UpdatedFieldSettingDto recoveryPlanUsed,
            UpdatedFieldSettingDto finishingDate,
            UpdatedFieldSettingDto attachedFile,
            UpdatedFieldSettingDto log,
            UpdatedFieldSettingDto implementationReady)
        {
            this.State = state;
            this.RealStartDate = realStartDate;
            this.BuildAndTextImplemented = buildAndTextImplemented;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.Deviation = deviation;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.FinishingDate = finishingDate;
            this.AttachedFile = attachedFile;
            this.Log = log;
            this.ImplementationReady = implementationReady;
        }

        [NotNull]
        public UpdatedFieldSettingDto State { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto RealStartDate { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto BuildAndTextImplemented { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto ImplementationPlanUsed { get; private set; }

        [NotNull]
        public UpdatedStringFieldSettingDto Deviation { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto RecoveryPlanUsed { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto FinishingDate { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto AttachedFile { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Log { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto ImplementationReady { get; private set; }
    }
}
