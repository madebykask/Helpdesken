namespace DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedImplementationFields
    {
        #region Constructors and Destructors

        public UpdatedImplementationFields(
            int? statusId,
            DateTime? realStartDate,
            DateTime? finishingDate,
            bool buildImplemented,
            bool implementationPlanUsed,
            string deviation,
            bool recoveryPlanUsed,
            bool implementationReady, 
            string logNote)
        {
            this.LogNote = logNote;
            this.StatusId = statusId;
            this.RealStartDate = realStartDate;
            this.FinishingDate = finishingDate;
            this.BuildImplemented = buildImplemented;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.Deviation = deviation;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.ImplementationReady = implementationReady;
        }

        #endregion

        #region Public Properties

        public bool BuildImplemented { get; private set; }

        public string Deviation { get; private set; }

        public DateTime? FinishingDate { get; private set; }

        public bool ImplementationPlanUsed { get; private set; }

        public bool ImplementationReady { get; private set; }

        public DateTime? RealStartDate { get; private set; }

        public bool RecoveryPlanUsed { get; private set; }

        [IsId]
        public int? StatusId { get; private set; }

        public string LogNote { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static UpdatedImplementationFields CreateEmpty()
        {
            return new UpdatedImplementationFields(null, null, null, false, false, null, false, false, string.Empty);
        }

        #endregion
    }
}