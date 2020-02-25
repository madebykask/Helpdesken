namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
	using DH.Helpdesk.BusinessData.Models;
	using DH.Helpdesk.Common.ValidationAttributes;
	using System.Collections.Generic;

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
            HistoryModel history, 
            OperationContext context,
			List<string> fileUploadWhiteList)
        {
            this.Context = context;
            this.IsNew = isNew;
            this.Id = id;
            this.Orderer = orderer;
            this.General = general;
            this.Registration = registration;
            this.Analyze = analyze;
            this.Implementation = implementation;
            this.Evaluation = evaluation;
            this.Log = log;
            this.History = history;
			this.FileUploadWhiteList = fileUploadWhiteList;
        }

        #endregion

        #region Public Properties

        public AnalyzeModel Analyze { get; set; }

        public EvaluationModel Evaluation { get; set; }

        [NotNull]
        public GeneralModel General { get; set; }

        public HistoryModel History { get; set; }

        [NotNullAndEmpty]
        public string Id { get; set; }

        public ImplementationModel Implementation { get; set; }

        public bool IsNew { get; set; }

        [NotNull]
        public LogModel Log { get; set; }

        [NotNull]
        public OrdererModel Orderer { get; set; }

        [NotNull]
        public RegistrationModel Registration { get; set; }

        [NotNull]
        public OperationContext Context { get; private set; }

		public List<string> FileUploadWhiteList { get; set; }

		#endregion
	}
}