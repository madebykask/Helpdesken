namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Responses.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface IAnalyzeModelFactory
    {
        #region Public Methods and Operators

        AnalyzeModel Create(FindChangeResponse response, ChangeEditData editData, AnalyzeEditSettings settings);

        #endregion
    }
}