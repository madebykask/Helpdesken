namespace DH.Helpdesk.Services.Infrastructure.BusinessModelEventsMailNotifiers
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Requests.Changes;
    using DH.Helpdesk.Services.Services;

    public sealed class ChangeEventsMailNotifier : IBusinessModelEventsMailNotifier<UpdateChangeRequest, Change>
    {
        private readonly IChangeEmailService changeEmailService;

        public ChangeEventsMailNotifier(IChangeEmailService changeEmailService)
        {
            this.changeEmailService = changeEmailService;
        }

        public void NotifyClients(UpdateChangeRequest request, Change existingChange)
        {
            if (request.AnalyzeNewLog != null && request.AnalyzeNewLog.Emails.Any())
            {
                this.changeEmailService.SendInternalLogNoteTo(
                    request.Change,
                    request.AnalyzeNewLog.Text,
                    request.AnalyzeNewLog.Emails,
                    request.CustomerId,
                    request.LanguageId);
            }

            if (request.ImplementationNewLog != null && request.ImplementationNewLog.Emails.Any())
            {
                this.changeEmailService.SendInternalLogNoteTo(
                    request.Change,
                    request.ImplementationNewLog.Text,
                    request.ImplementationNewLog.Emails,
                    request.CustomerId,
                    request.LanguageId);
            }

            if (request.EvaluationNewLog != null && request.EvaluationNewLog.Emails.Any())
            {
                this.changeEmailService.SendInternalLogNoteTo(
                    request.Change,
                    request.EvaluationNewLog.Text,
                    request.EvaluationNewLog.Emails,
                    request.CustomerId,
                    request.LanguageId);
            }
        }
    }
}
