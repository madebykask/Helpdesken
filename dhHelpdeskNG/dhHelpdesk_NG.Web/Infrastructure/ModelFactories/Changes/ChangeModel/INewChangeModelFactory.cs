namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface INewChangeModelFactory
    {
        #region Public Methods and Operators

        InputModel Create(string temporatyId, ChangeEditData editData, ChangeEditSettings editSettings);

        #endregion
    }
}