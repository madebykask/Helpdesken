namespace DH.Helpdesk.Web.Models.Changes.ChangesGrid
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Common;

    public sealed class ChangeOverviewModel
    {
        #region Constructors and Destructors

        public ChangeOverviewModel(
            int id,
            StepStatus registrationStepStatus,
            StepStatus analyzeStepStatus,
            StepStatus implementationStepStatus,
            StepStatus evaluationStepStatus,
            List<NewGridRowCellValueModel> fieldValues)
        {
            this.Id = id;
            this.RegistrationStepStatus = registrationStepStatus;
            this.AnalyzeStepStatus = analyzeStepStatus;
            this.ImplementationStepStatus = implementationStepStatus;
            this.EvaluationStepStatus = evaluationStepStatus;
            this.FieldValues = fieldValues;
        }

        #endregion

        #region Public Properties

        public StepStatus AnalyzeStepStatus { get; set; }

        public StepStatus EvaluationStepStatus { get; set; }

        [NotNull]
        public List<NewGridRowCellValueModel> FieldValues { get; set; }

        [IsId]
        public int Id { get; set; }

        public StepStatus ImplementationStepStatus { get; set; }

        public StepStatus RegistrationStepStatus { get; set; }

        #endregion
    }
}