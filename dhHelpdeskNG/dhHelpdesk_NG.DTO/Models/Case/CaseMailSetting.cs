
namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseMailSetting
    {
        public CaseMailSetting() {}

        public CaseMailSetting(string sendEmailAboutNewCaseTo, string helpdeskMailFromAdress, string absoluterUrl)
        {
            this.SendMailAboutNewCaseTo = sendEmailAboutNewCaseTo;
            this.HelpdeskMailFromAdress = helpdeskMailFromAdress;
            this.AbsoluterUrl = absoluterUrl;
        }

        public bool DontSendMailToNotifier { get; set; }
        public string SendMailAboutNewCaseTo { get; set; }
        public string HelpdeskMailFromAdress { get; set; }
        public string AbsoluterUrl { get; set; }
    }
}
