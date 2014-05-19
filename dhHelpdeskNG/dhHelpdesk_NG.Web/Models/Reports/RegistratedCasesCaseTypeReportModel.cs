namespace DH.Helpdesk.Web.Models.Reports
{
    public sealed class RegistratedCasesCaseTypeReportModel
    {
        public RegistratedCasesCaseTypeReportModel(string key)
        {
            this.Key = key;
        }

        public string Key { get; private set; }
    }
}