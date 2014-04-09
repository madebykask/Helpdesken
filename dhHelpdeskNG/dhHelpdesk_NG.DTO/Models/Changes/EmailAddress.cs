namespace DH.Helpdesk.BusinessData.Models.Changes
{
    using System.Net.Mail;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class EmailAddress
    {
        public EmailAddress(EmailKind kind, MailAddress address)
        {
            this.Kind = kind;
            this.Address = address;
        }

        public EmailKind Kind { get; private set; }

        [NotNull]
        public MailAddress Address { get; private set; }
    }
}