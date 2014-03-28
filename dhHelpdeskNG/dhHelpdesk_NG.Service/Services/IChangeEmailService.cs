namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;

    public interface IChangeEmailService
    {
        void SendInternalLogNoteTo(UpdatedChange change, string text, List<string> emails, int customerId, int languageId);
    }
}