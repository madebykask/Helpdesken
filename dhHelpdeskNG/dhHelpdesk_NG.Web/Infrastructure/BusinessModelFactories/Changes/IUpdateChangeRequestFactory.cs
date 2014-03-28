namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface IUpdateChangeRequestFactory
    {
        UpdateChangeRequest Create(
            InputModel model,
            List<string> deletedRegistrationFiles,
            List<string> deletedAnalyzeFiles,
            List<string> deletedImplementationFiles,
            List<string> deletedEvaluationFiles,
            List<int> deletedLogIds,
            List<WebTemporaryFile> newRegistrationFiles,
            List<WebTemporaryFile> newAnalyzeFiles,
            List<WebTemporaryFile> newImplementationFiles,
            List<WebTemporaryFile> newEvaluationFiles,
            int currentUserId,
            int currentCustomerId,
            int currentLangugeId,
            DateTime changedDateAndTime);
    }
}