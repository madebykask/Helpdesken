namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Mobile.Models.Changes.ChangeEdit;

    public interface INewOrdererModelFactory
    {
        #region Public Methods and Operators

        OrdererModel Create(OrdererEditSettings settings, ChangeEditOptions options);

        #endregion
    }
}