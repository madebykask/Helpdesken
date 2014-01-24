namespace dhHelpdesk_NG.Web.Models
{
    public class CaseOverview
    {
        public int Id { get; set; }

        public string CaseNumber { get; set; }

        public string RegistrationDate { get; set; }

        public string Caption { get; set; }

        public string SubState { get; set; }

        public string CaseType { get; set; }

        public string WatchDate { get; set; }
    }
}