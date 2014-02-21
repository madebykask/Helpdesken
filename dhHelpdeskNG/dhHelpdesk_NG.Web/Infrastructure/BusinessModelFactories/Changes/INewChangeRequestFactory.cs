namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Requests.Changes;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface INewChangeRequestFactory
    {
        NewChangeRequest Create(
            InputModel model,
            List<WebTemporaryFile> registrationFiles,
            int currentUserId,
            int currentCustomerId,
            int currentLanguageId,
            DateTime createdDateAndTime);
    }
}
