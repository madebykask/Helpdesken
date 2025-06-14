namespace DH.Helpdesk.Web.Models.Questionnaire.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class CircularOverviewViewModel
    {
        public CircularOverviewViewModel(
            int questionnaireId,
            List<CircularOverviewModel> circularOverviewModels,
            int circularStateId,
            ReportFilter reportFilter)
        {
            this.QuestionnaireId = questionnaireId;
            this.CircularOverviewModels = circularOverviewModels;
            this.CircularStateId = circularStateId;
            this.ReportFilter = reportFilter;
        }

        public int QuestionnaireId { get; set; }

        [NotNull]
        public List<CircularOverviewModel> CircularOverviewModels { get; set; }

        public int CircularStateId { get; set; }

        public ReportFilter ReportFilter { get; set; }
    }
}