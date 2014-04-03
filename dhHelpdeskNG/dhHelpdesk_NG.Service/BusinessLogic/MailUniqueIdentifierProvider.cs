namespace DH.Helpdesk.Services.BusinessLogic
{
    using System;
    using System.Net.Mail;

    public sealed class MailUniqueIdentifierProvider : IMailUniqueIdentifierProvider
    {
        private const string IdentifierPattern = "[{0}][{1}]@{2}";

        public string Provide(DateTime dateCeed, MailAddress emailCeed)
        {
            var utcDateAndTime = dateCeed.ToUniversalTime();
            var guid = Guid.NewGuid();
            var host = emailCeed.Host;

            return string.Format(IdentifierPattern, utcDateAndTime, guid, host);
        }
    }
}