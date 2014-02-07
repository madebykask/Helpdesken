namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChangeAggregate;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Changes;

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
