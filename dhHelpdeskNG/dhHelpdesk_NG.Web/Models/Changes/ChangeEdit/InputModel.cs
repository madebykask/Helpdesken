namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class InputModel
    {
        #region Constructors and Destructors

        public InputModel()
        {
        }

        public InputModel(
            bool isNew,
            string id,
            OrdererModel orderer,
            GeneralModel general,
            RegistrationModel registration,
            AnalyzeModel analyze,
            ImplementationModel implementation,
            EvaluationModel evaluation,
            LogModel log,
            HistoryModel history)
        {
            this.IsNew = isNew;
            this.Id = id;
            this.OrdererModel = orderer;
            this.GeneralModel = general;
            this.RegistrationModel = registration;
            this.AnalyzeModel = analyze;
            this.ImplementationModel = implementation;
            this.Evaluation = evaluation;
            this.Log = log;
            this.History = history;
        }

        #endregion

        #region Public Properties

        public AnalyzeModel AnalyzeModel { get; set; }

        public EvaluationModel Evaluation { get; set; }

        [NotNull]
        public GeneralModel GeneralModel { get; set; }

        public HistoryModel History { get; set; }

        [NotNullAndEmpty]
        public string Id { get; set; }

        public ImplementationModel ImplementationModel { get; set; }

        public bool IsNew { get; set; }

        [NotNull]
        public LogModel Log { get; set; }

        [NotNull]
        public OrdererModel OrdererModel { get; set; }

        [NotNull]
        public RegistrationModel RegistrationModel { get; set; }

        #endregion
    }
}