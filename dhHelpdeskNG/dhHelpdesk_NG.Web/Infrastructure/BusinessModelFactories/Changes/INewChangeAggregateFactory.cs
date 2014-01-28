namespace dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Changes
{
    using System;
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChangeAggregate;
    using dhHelpdesk_NG.Web.Infrastructure.Tools;
    using dhHelpdesk_NG.Web.Models.Changes;

    public interface INewChangeAggregateFactory
    {
        NewChangeAggregate Create(
            NewChangeModel model,
            List<WebTemporaryFile> registrationFiles,
            List<WebTemporaryFile> analyzeFiles,
            List<WebTemporaryFile> implementationFiles,
            List<WebTemporaryFile> evaluationFiles,
            int customerId,
            DateTime createdDate);
    }
}
