namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class NewChangeModelFactory : INewChangeModelFactory
    {
        private readonly INewOrdererModelFactory newOrdererModelFactory;

        private readonly INewGeneralModelFactory newGeneralModelFactory;

        private readonly INewRegistrationModelFactory newRegistrationModelFactory;

        private readonly INewLogModelFactory newLogModelFactory;

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

        public InputModel Create(string temporatyId, ChangeEditData editData, ChangeEditSettings settings)
        {
            var orderer = this.newOrdererModelFactory.Create(editData, settings.Orderer);
            var general = this.newGeneralModelFactory.Create(editData, settings.General);
            var registration = this.newRegistrationModelFactory.Create(temporatyId, editData, settings.Registration);
            var log = this.newLogModelFactory.Create(temporatyId, editData, settings.Log);

            return new InputModel(temporatyId, true, orderer, general, registration, null, null, null, log, null);
        }
    }
}