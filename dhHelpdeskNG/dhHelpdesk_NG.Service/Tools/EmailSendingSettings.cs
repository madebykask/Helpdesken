namespace DH.Helpdesk.Services.Tools
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class EmailSendingSettings
    {
        public EmailSendingSettings(int smtpPort, string smtpServer)
        {
            this.SmtpPort = smtpPort;
            this.SmtpServer = smtpServer;
        }

        [NotNullAndEmpty]
        public int SmtpPort { get; private set; }

        [NotNullAndEmpty]
        public string SmtpServer { get; private set; }
    }
}