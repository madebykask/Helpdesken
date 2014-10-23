
namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseMailSetting
    {
        public CaseMailSetting() {}

        public CaseMailSetting(string sendEmailAboutNewCaseTo, string helpdeskMailFromAdress, string absoluterUrl, int dontConnectUserToWorkingGroup)
        {
            this.SendMailAboutNewCaseTo = sendEmailAboutNewCaseTo;
            this.HelpdeskMailFromAdress = helpdeskMailFromAdress;
            this.AbsoluterUrl = absoluterUrl;
            this.DontConnectUserToWorkingGroup = dontConnectUserToWorkingGroup;            
        }

        public int DontConnectUserToWorkingGroup { get; set; }
        public bool DontSendMailToNotifier { get; set; }
        public string SendMailAboutNewCaseTo { get; set; }
        public string HelpdeskMailFromAdress { get; set; }
        public string AbsoluterUrl { get; set; }
        public MailSenders CustomeMailFromAddress { get; set; } // used for Mailtemplate (1,3,4)
    }

    public class MailSenders
    {
        public MailSenders() 
        {
        }

        public string DefaultOwnerWGEMail { get; set; } 
        public string WGEmail { get; set; } 
        public string SystemEmail { get; set; } 

    }
}
