namespace DH.Helpdesk.BusinessData.Models.Email
{
    using System;

    public sealed class EmailSettings
    {
        public EmailSettings()
        {
        }

        public EmailSettings(EmailResponse response, MailSMTPSetting smtpSettings, bool batchEmail)
        {
            Response = response;
            SmtpSettings = smtpSettings;
	        BatchEmail = batchEmail;
        }

        public EmailResponse Response { get; private set; }

        public MailSMTPSetting SmtpSettings { get; private set; }

		public bool BatchEmail { get; set; }

    }

    public sealed class EmailResponse
    {
        public EmailResponse()
        {            
        }

        public EmailResponse(DateTime? sendTime, string responseMessage, int numberOfTry)
        {
            SendTime = sendTime;
            ResponseMessage = responseMessage;
            NumberOfTry = numberOfTry;           
        }        
        
        public DateTime? SendTime { get; set; }
        
        public string ResponseMessage { get; set; }

        public int NumberOfTry { get; set; }

        public MailSMTPSetting SmtpSettings { get; set; }

        public static EmailResponse GetEmptyEmailResponse()
        {
            return new EmailResponse(DateTime.Now, string.Empty, 1);
        }

    }

    public sealed class MailSMTPSetting
    {
        public MailSMTPSetting(string server, int port, string userName = "", string pass = "", bool isSecured = false)
        {
            Server = server;
            Port = port;
            UserName = userName;
            Pass = pass;
            IsSecured = isSecured;
        }
        public string Server { get; private set; }

        public int Port { get; private set; }

        public string UserName { get; private set; }

        public string Pass  { get; private set; }

        public bool IsSecured { get; private set; }

    }
}