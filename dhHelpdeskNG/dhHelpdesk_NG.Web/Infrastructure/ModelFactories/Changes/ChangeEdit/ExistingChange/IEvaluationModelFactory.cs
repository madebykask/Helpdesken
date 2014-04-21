namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange
{
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface IEvaluationModelFactory
    {
        #region Public Methods and Operators

        EvaluationModel Create(FindChangeResponse response);

        #endregion
    }
}