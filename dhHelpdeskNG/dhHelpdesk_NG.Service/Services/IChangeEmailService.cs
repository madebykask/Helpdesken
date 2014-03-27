namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;

    public interface IChangeEmailService
    {
        void SendInternalLogNoteTo(Change change, string text, List<string> emails, int customerId, int languageId);
    }
}