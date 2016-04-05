using Microsoft.Reporting.WebForms;
namespace DH.Helpdesk.Web.Models.Shared
{
    public sealed class ReportModel
    {
        #region Constructors and Destructors

        public ReportModel()
        {            
        }

        public ReportModel(bool canShow)
        {
            CanShow = canShow;
        }

        public bool CanShow { get; set; }

        #endregion

        public ReportViewer Report { get; set; }
    }
}