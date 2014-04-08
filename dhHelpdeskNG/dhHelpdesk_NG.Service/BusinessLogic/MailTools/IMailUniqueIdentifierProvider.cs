namespace DH.Helpdesk.Services.BusinessLogic.MailTools
{
    using System;
    using System.Net.Mail;

    public interface IMailUniqueIdentifierProvider
    {
        string Provide(DateTime dateCeed, MailAddress emailCeed);
    }
}