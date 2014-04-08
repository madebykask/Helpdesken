namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface IEvaluationModelFactory
    {
        #region Public Methods and Operators

        EvaluationModel Create(
            FindChangeResponse response, ChangeEditData editData, EvaluationEditSettings settings);

        #endregion
    }
}