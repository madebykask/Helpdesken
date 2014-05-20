﻿namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange.Concrete
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class NewChangeModelFactory : INewChangeModelFactory
    {
        #region Fields

        private readonly INewGeneralModelFactory newGeneralModelFactory;

        private readonly INewLogModelFactory newLogModelFactory;

        private readonly INewOrdererModelFactory newOrdererModelFactory;

        private readonly INewRegistrationModelFactory newRegistrationModelFactory;

        #endregion

        #region Constructors and Destructors

        public NewChangeModelFactory(
            INewOrdererModelFactory newOrdererModelFactory,
            INewGeneralModelFactory newGeneralModelFactory,
            INewRegistrationModelFactory newRegistrationModelFactory,
            INewLogModelFactory newLogModelFactory)
        {
            this.newOrdererModelFactory = newOrdererModelFactory;
            this.newGeneralModelFactory = newGeneralModelFactory;
            this.newRegistrationModelFactory = newRegistrationModelFactory;
            this.newLogModelFactory = newLogModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public InputModel Create(
                        string temporatyId, 
                        GetNewChangeEditDataResponse response,
                        OperationContext context)
        {
            var orderer = this.newOrdererModelFactory.Create(response.EditSettings.Orderer, response.EditOptions);
            var general = this.newGeneralModelFactory.Create(response.EditSettings.General, response.EditOptions);

            var registration = this.newRegistrationModelFactory.Create(
                temporatyId,
                response.EditSettings.Registration, response.EditOptions);

            var log = this.newLogModelFactory.Create(temporatyId, response.EditSettings.Log, response.EditOptions);

            return new InputModel(true, temporatyId, orderer, general, registration, null, null, null, log, null, context);
        }

        #endregion
    }
}