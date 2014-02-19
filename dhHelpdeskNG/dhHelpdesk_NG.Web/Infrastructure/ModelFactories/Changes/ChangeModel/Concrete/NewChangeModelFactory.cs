namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class NewChangeModelFactory : INewChangeModelFactory
    {
        private readonly INewOrdererModelFactory ordererModelFactory;

        private readonly INewGeneralModelFactory generalModelFactory;

        private readonly INewRegistrationModelFactory registrationModelFactory;

        public NewChangeModelFactory(
            INewOrdererModelFactory ordererModelFactory,
            INewGeneralModelFactory generalModelFactory,
            INewRegistrationModelFactory registrationModelFactory)
        {
            this.ordererModelFactory = ordererModelFactory;
            this.generalModelFactory = generalModelFactory;
            this.registrationModelFactory = registrationModelFactory;
        }

        public InputModel Create(string temporatyId, ChangeEditData editData, ChangeEditSettings settings)
        {
            var orderer = this.ordererModelFactory.Create(editData, settings.Orderer);
            var general = this.generalModelFactory.Create(editData, settings.General);
            var registration = this.registrationModelFactory.Create(temporatyId, editData, settings.Registration);

            return new InputModel(temporatyId, true, orderer, general, registration, null, null, null, null);
        }
    }
}