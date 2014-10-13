namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Mobile.Models.Changes.ChangeEdit;

    public interface INewLogModelFactory
    {
        #region Public Methods and Operators

        LogModel Create(string temporaryId, LogEditSettings settings, ChangeEditOptions options);

        #endregion
    }
}