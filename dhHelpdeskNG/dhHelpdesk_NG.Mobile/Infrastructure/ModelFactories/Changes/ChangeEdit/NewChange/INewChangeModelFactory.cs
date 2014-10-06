namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Domain.Changes;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Mobile.Models.Changes.ChangeEdit;

    public interface INewChangeModelFactory
    {
        #region Public Methods and Operators

        InputModel Create(
                string temporatyId, 
                GetNewChangeEditDataResponse response,
                OperationContext context,
                IList<ChangeStatusEntity> statuses);

        #endregion
    }
}