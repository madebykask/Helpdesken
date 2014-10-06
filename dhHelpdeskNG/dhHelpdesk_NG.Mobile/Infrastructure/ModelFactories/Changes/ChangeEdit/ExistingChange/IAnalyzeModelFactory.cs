namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange
{
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface IAnalyzeModelFactory
    {
        #region Public Methods and Operators

        AnalyzeModel Create(FindChangeResponse response);

        #endregion
    }
}