namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class InputModel
    {
        public InputModel()
        {
        }

        public InputModel(
            string changeId,
            bool isNew,
            OrdererModel orderer,
            GeneralModel general,
            RegistrationModel registration,
            AnalyzeModel analyze,
            ImplementationModel implementation,
            EvaluationModel evaluation,
            HistoriesModel history)
        {
            this.ChangeId = changeId;
            this.IsNew = isNew;
            this.Orderer = orderer;
            this.General = general;
            this.Registration = registration;
            this.Analyze = analyze;
            this.Implementation = implementation;
            this.Evaluation = evaluation;
            this.History = history;
        }

        [NotNullAndEmpty]
        public string ChangeId { get; private set; }

        public bool IsNew { get; private set; }

        [NotNull]
        public OrdererModel Orderer { get; set; }

        [NotNull]
        public GeneralModel General { get; set; }

        [NotNull]
        public RegistrationModel Registration { get; set; }

        public AnalyzeModel Analyze { get; set; }

        public ImplementationModel Implementation { get; set; }

        public EvaluationModel Evaluation { get; set; }

        public HistoriesModel History { get; set; }
    }
}