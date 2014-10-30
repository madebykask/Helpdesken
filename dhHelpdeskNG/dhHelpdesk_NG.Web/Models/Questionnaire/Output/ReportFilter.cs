namespace DH.Helpdesk.Web.Models.Questionnaire.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public class ReportFilter
    {
        public ReportFilter()
        {
        }

        public ReportFilter(List<int> connectedCirculars)
        {
            this.ConnectedCirculars = connectedCirculars;
            this.CircularCreatedDate = new DateRange();
        }

        [NotNull]
        public List<int> ConnectedCirculars { get; set; }

        [NotNull]
        public DateRange CircularCreatedDate { get; set; }
    }
}