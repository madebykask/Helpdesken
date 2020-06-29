namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange.Concrete
{
	using System.Collections.Generic;

	using DH.Helpdesk.BusinessData.Models;
	using DH.Helpdesk.Domain.Changes;
	using DH.Helpdesk.Services.Response.Changes;
	using DH.Helpdesk.Web.Models.Changes.ChangeEdit;
	using Services.Services;

	public sealed class NewChangeModelFactory : INewChangeModelFactory
    {
        #region Fields

        private readonly INewGeneralModelFactory _newGeneralModelFactory;
        private readonly INewLogModelFactory _newLogModelFactory;
        private readonly INewOrdererModelFactory _newOrdererModelFactory;
        private readonly INewRegistrationModelFactory _newRegistrationModelFactory;

		#endregion

		#region Constructors and Destructors

		public NewChangeModelFactory(
            INewOrdererModelFactory newOrdererModelFactory,
            INewGeneralModelFactory newGeneralModelFactory,
            INewRegistrationModelFactory newRegistrationModelFactory,
            INewLogModelFactory newLogModelFactory)
        {
            this._newOrdererModelFactory = newOrdererModelFactory;
            this._newGeneralModelFactory = newGeneralModelFactory;
            this._newRegistrationModelFactory = newRegistrationModelFactory;
            this._newLogModelFactory = newLogModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public InputModel Create(
                        string temporatyId, 
                        GetNewChangeEditDataResponse response,
                        OperationContext context,
                        IList<ChangeStatusEntity> statuses,
						List<string> fileUploadWhiteList)
        {
            var orderer = this._newOrdererModelFactory.Create(response.EditSettings.Orderer, response.EditOptions);
            var general = this._newGeneralModelFactory.Create(
                                    response.EditSettings.General, 
                                    response.EditOptions,
                                    context,
                                    statuses);

            var registration = this._newRegistrationModelFactory.Create(
                                    temporatyId,
                                    response.EditSettings.Registration, 
                                    response.EditOptions);

            var log = this._newLogModelFactory.Create(temporatyId, response.EditSettings.Log, response.EditOptions);

            return new InputModel(true, temporatyId, orderer, general, registration, null, null, null, log, null, context, fileUploadWhiteList);
        }

        #endregion
    }
}