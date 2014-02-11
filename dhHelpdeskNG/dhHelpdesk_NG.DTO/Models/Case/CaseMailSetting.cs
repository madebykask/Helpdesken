
namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseMailSetting
    {
        public CaseMailSetting() {}

        public CaseMailSetting(string sendEmailAboutNewCaseTo, string helpdeskMailFromAdress)
        {
            this.SendMailAboutNewCaseTo = sendEmailAboutNewCaseTo;
            this.HelpdeskMailFromAdress = helpdeskMailFromAdress; 
        }

        public bool DontSendMailToNotifier { get; set; }
        public string SendMailAboutNewCaseTo { get; set; }
        public string HelpdeskMailFromAdress { get; set; }
    }
}
