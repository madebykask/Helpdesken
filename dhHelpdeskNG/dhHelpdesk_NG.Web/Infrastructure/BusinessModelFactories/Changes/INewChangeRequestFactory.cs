using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

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