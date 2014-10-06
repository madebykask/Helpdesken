namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Domain.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface INewGeneralModelFactory
    {
        #region Public Methods and Operators

        GeneralModel Create(
                GeneralEditSettings settings, 
                ChangeEditOptions options,
                OperationContext context,
                IList<ChangeStatusEntity> statuses);

        #endregion
    }
}