﻿namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface IChangeModelFactory
    {
        #region Public Methods and Operators

        InputModel Create(
            FindChangeResponse response,
            OperationContext context);

        #endregion
    }
}