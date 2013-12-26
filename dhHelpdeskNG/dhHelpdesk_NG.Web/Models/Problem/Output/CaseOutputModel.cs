namespace dhHelpdesk_NG.Web.Models.Problem.Output
{
    public class CaseOutputModel
    {
        public int Id { get; set; }

        public string CaseNumber { get; set; }

        public string RegistrationDate { get; set; }

        public string Department { get; set; }

        public string Administrator { get; set; }

        public string Caption { get; set; }

        public string Priority { get; set; }

        public string SubState { get; set; }

        public string WorkingGroup { get; set; }
    }
}