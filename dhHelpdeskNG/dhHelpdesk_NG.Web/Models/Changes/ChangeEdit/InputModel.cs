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
            OrdererViewModel ordererViewModel,
            GeneralViewModel generalViewModel,
            RegistrationViewModel registrationViewModel,
            AnalyzeViewModel analyzeViewModel,
            ImplementationViewModel implementationViewModel,
            EvaluationModel evaluation,
            LogModel log,
            HistoriesModel history)
        {
            this.ChangeId = changeId;
            this.IsNew = isNew;
            this.OrdererViewModel = ordererViewModel;
            this.GeneralViewModel = generalViewModel;
            this.RegistrationViewModel = registrationViewModel;
            this.AnalyzeViewModel = analyzeViewModel;
            this.ImplementationViewModel = implementationViewModel;
            this.Evaluation = evaluation;
            this.Log = log;
            this.History = history;
        }

        [NotNullAndEmpty]
        public string ChangeId { get; set; }

        public bool IsNew { get; private set; }

        [NotNull]
        public OrdererViewModel OrdererViewModel { get; set; }

        [NotNull]
        public GeneralViewModel GeneralViewModel { get; set; }

        [NotNull]
        public RegistrationViewModel RegistrationViewModel { get; set; }

        public AnalyzeViewModel AnalyzeViewModel { get; set; }

        public ImplementationViewModel ImplementationViewModel { get; set; }

        public EvaluationModel Evaluation { get; set; }

        public LogModel Log { get; set; }

        public HistoriesModel History { get; set; }
    }
}