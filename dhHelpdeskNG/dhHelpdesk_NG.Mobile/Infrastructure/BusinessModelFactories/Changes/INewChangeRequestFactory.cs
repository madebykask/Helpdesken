namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Mobile.Infrastructure.Tools;
    using DH.Helpdesk.Mobile.Models.Changes.ChangeEdit;

    public interface INewChangeRequestFactory
    {
        #region Public Methods and Operators

        NewChangeRequest Create(
                    InputModel model, 
                    List<WebTemporaryFile> registrationFiles, 
                    OperationContext context,
                    IEmailService emailService);

        #endregion
    }
}