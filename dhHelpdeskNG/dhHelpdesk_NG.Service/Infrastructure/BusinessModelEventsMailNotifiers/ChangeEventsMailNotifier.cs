namespace DH.Helpdesk.Services.Infrastructure.BusinessModelEventsMailNotifiers
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Services.Services;

    public sealed class ChangeEventsMailNotifier : IBusinessModelEventsMailNotifier<UpdateChangeRequest, Change>
    {
        private readonly IUserEmailRepository userEmailRepository;

        private readonly IChangeEmailService changeEmailService;

        public ChangeEventsMailNotifier(
            IChangeEmailService changeEmailService,
            IUserEmailRepository userEmailRepository)
        {
            this.changeEmailService = changeEmailService;
            this.userEmailRepository = userEmailRepository;
        }

        public void NotifyClients(UpdateChangeRequest updated, Change existing)
        {
            this.NotifySendLogNoteTo(updated, existing);
            this.NotifyStatusChanged(updated, existing);
            this.NotifyAssignedToUser(updated, existing);
        }

        private void NotifySendLogNoteTo(UpdateChangeRequest request, Change existingChange)
        {
            foreach (var manualLog in request.NewLogs)
            {
                this.changeEmailService.SendInternalLogNoteTo(
                    request.Change,
                    manualLog.Text,
                    manualLog.Emails,
                    request.Context.CustomerId.Value,
                    request.Context.LanguageId.Value);
            }
        }

        private void NotifyAssignedToUser(UpdateChangeRequest request, Change existingChange)
        {
            if (request.Change.General.AdministratorId == null)
            {
                return;
            }

            if (request.Change.General.AdministratorId == existingChange.General.AdministratorId)
            {
                return;
            }

            var assignedUserEmails =
                this.userEmailRepository.FindUserEmails(request.Change.General.AdministratorId.Value);

            this.changeEmailService.SendAssignedToUser(
                request.Change,
                assignedUserEmails,
                request.Context.CustomerId.Value,
                request.Context.LanguageId.Value);
        }

        private void NotifyStatusChanged(UpdateChangeRequest request, Change existingChange)
        {
            if (request.Change.General.AdministratorId == null)
            {
                return;
            }

            if (request.Change.General.StatusId == existingChange.General.StatusId)
            {
                return;
            }

            var ownerEmails = this.userEmailRepository.FindUserEmails(request.Change.General.AdministratorId.Value);

            this.changeEmailService.SendStatusChanged(
                request.Change,
                ownerEmails,
                request.Context.CustomerId.Value,
                request.Context.LanguageId.Value);
        }

        private void NotifyChangeChanged(UpdateChangeRequest request, Change existingChange)
        {
            throw new NotImplementedException();
        }
    }
}