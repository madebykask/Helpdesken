namespace DH.Helpdesk.Web.Models.Changes.ChangesGrid
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Common;

    public sealed class ChangeOverviewModel
    {
        public ChangeOverviewModel(
            int id,
            StepStatus registrationStepStatus,
            StepStatus analyzeStepStatus,
            StepStatus implementationStepStatus,
            StepStatus evaluationStepStatus,
            List<GridRowCellValueModel> fieldValues)
        {
            this.Id = id;
            this.RegistrationStepStatus = registrationStepStatus;
            this.AnalyzeStepStatus = analyzeStepStatus;
            this.ImplementationStepStatus = implementationStepStatus;
            this.EvaluationStepStatus = evaluationStepStatus;
            this.FieldValues = fieldValues;
        }

        [IsId]
        public int Id { get; set; }

        public StepStatus RegistrationStepStatus { get; set; }

        public StepStatus AnalyzeStepStatus { get; set; }

        public StepStatus ImplementationStepStatus { get; set; }

        public StepStatus EvaluationStepStatus { get; set; }

        [NotNull]
        public List<GridRowCellValueModel> FieldValues { get; set; }
    }
}