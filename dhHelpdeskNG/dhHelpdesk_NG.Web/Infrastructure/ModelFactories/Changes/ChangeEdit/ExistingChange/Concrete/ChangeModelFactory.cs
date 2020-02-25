namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
	using System.Globalization;

	using DH.Helpdesk.BusinessData.Models;
	using DH.Helpdesk.Services.Response.Changes;
	using DH.Helpdesk.Web.Models.Changes.ChangeEdit;
	using Services.Services;

	public sealed class ChangeModelFactory : IChangeModelFactory
    {
        #region Fields

        private readonly IAnalyzeModelFactory analyzeModelFactory;

        private readonly IEvaluationModelFactory evaluationModelFactory;

        private readonly IGeneralModelFactory generalModelFactory;

        private readonly IHistoryModelFactory historyModelFactory;

        private readonly IImplementationModelFactory implementationModelFactory;

        private readonly ILogModelFactory logModelFactory;

        private readonly IOrdererModelFactory ordererModelFactory;

        private readonly IRegistrationModelFactory registrationModelFactory;

		#endregion

		#region Constructors and Destructors

		public ChangeModelFactory(
            IAnalyzeModelFactory analyzeModelFactory,
            IEvaluationModelFactory evaluationModelFactory,
            IGeneralModelFactory generalModelFactory,
            IImplementationModelFactory implementationModelFactory,
            IOrdererModelFactory ordererModelFactory,
            IRegistrationModelFactory registrationModelFactory,
            IHistoryModelFactory historyModelFactory,
            ILogModelFactory logModelFactory)
        {
            this.analyzeModelFactory = analyzeModelFactory;
            this.evaluationModelFactory = evaluationModelFactory;
            this.generalModelFactory = generalModelFactory;
            this.implementationModelFactory = implementationModelFactory;
            this.ordererModelFactory = ordererModelFactory;
            this.registrationModelFactory = registrationModelFactory;
            this.historyModelFactory = historyModelFactory;
            this.logModelFactory = logModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public InputModel Create(
                FindChangeResponse response,
                OperationContext context,
				IGlobalSettingService globalSettingsService)
        {
            var textId = response.EditData.Change.Id.ToString(CultureInfo.InvariantCulture);

            var orderer = this.ordererModelFactory.Create(response);
            var general = this.generalModelFactory.Create(response);
            var registration = this.registrationModelFactory.Create(response);
            var analyze = this.analyzeModelFactory.Create(response);
            var implementation = this.implementationModelFactory.Create(response);
            var evaluation = this.evaluationModelFactory.Create(response);
            var log = this.logModelFactory.Create(response);
            var history = this.historyModelFactory.Create(response);
			var fileUploadWhiteList = globalSettingsService.GetFileUploadWhiteList();

            return new InputModel(
                false,
                textId,
                orderer,
                general,
                registration,
                analyze,
                implementation,
                evaluation,
                log,
                history,
                context,
				fileUploadWhiteList);
        }

        #endregion
    }
}