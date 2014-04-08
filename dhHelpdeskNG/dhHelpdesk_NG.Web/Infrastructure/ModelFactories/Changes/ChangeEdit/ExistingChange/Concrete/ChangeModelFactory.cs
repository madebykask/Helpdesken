﻿namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
    using System.Globalization;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.Shared;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class ChangeModelFactory : IChangeModelFactory
    {
        #region Fields

        private readonly IAnalyzeModelFactory analyzeModelFactory;

        private readonly IEvaluationModelFactory evaluationModelFactory;

        private readonly IGeneralModelFactory generalModelFactory;

        private readonly IImplementationModelFactory implementationModelFactory;

        private readonly IOrdererModelFactory ordererModelFactory;

        private readonly IRegistrationModelFactory registrationModelFactory;

        private readonly ILogModelFactory logModelFactory;

        private readonly IHistoriesModelFactory historiesModelFactory;

        public ChangeModelFactory(
            IAnalyzeModelFactory analyzeModelFactory,
            IEvaluationModelFactory evaluationModelFactory,
            IGeneralModelFactory generalModelFactory,
            IImplementationModelFactory implementationModelFactory,
            IOrdererModelFactory ordererModelFactory,
            IRegistrationModelFactory registrationModelFactory,
            IHistoriesModelFactory historiesModelFactory,
            ILogModelFactory logModelFactory)
        {
            this.analyzeModelFactory = analyzeModelFactory;
            this.evaluationModelFactory = evaluationModelFactory;
            this.generalModelFactory = generalModelFactory;
            this.implementationModelFactory = implementationModelFactory;
            this.ordererModelFactory = ordererModelFactory;
            this.registrationModelFactory = registrationModelFactory;
            this.historiesModelFactory = historiesModelFactory;
            this.logModelFactory = logModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public InputModel Create(FindChangeResponse response, ChangeEditData editData, ChangeEditSettings settings)
        {
            var textId = response.Change.Id.ToString(CultureInfo.InvariantCulture);

            var orderer = this.ordererModelFactory.Create(response, editData, settings.Orderer);
            var general = this.generalModelFactory.Create(response, editData, settings.General);
            var registration = this.registrationModelFactory.Create(response, editData, settings.Registration);
            var analyze = this.analyzeModelFactory.Create(response, editData, settings.Analyze);
            var implementation = this.implementationModelFactory.Create(response, editData, settings.Implementation);
            var evaluation = this.evaluationModelFactory.Create(response, editData, settings.Evaluation);
            var log = this.logModelFactory.Create(response, editData, settings.Log);
            var history = this.historiesModelFactory.Create(response);

            return new InputModel(
                textId,
                false,
                orderer,
                general,
                registration,
                analyze,
                implementation,
                evaluation,
                log,
                history);
        }

        #endregion
    }
}