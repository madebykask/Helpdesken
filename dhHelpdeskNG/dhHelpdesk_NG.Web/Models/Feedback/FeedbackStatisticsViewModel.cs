using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Read;
using DH.Helpdesk.Services.Response.Questionnaire;
using DH.Helpdesk.Web.Models.Case;
using DH.Helpdesk.Web.Models.Questionnaire.Output;

namespace DH.Helpdesk.Web.Models.Feedback
{
	public class FeedbackStatisticsViewModel : StatisticsViewModel
	{
		public FeedbackStatisticsViewModel(
			int questionnaireId,
			QuestionnaireOverview questionnaireOverview,
			List<OptionResult> optionResults,
			StatisticsFilter statisticsFilter,
            JsonCaseIndexViewModel caseIndexViewModel) : base(questionnaireId, questionnaireOverview, optionResults)
		{
			this.QuestionnaireId = questionnaireId;
			this.QuestionnaireOverview = questionnaireOverview;
			this.OptionResults = optionResults;
			this.StatisticsFilter = statisticsFilter;
			this.FeedbackStatisticsCases = caseIndexViewModel;
            Emails = new List<string>();
		}

		public StatisticsFilter StatisticsFilter { get; set; }

        public JsonCaseIndexViewModel FeedbackStatisticsCases { get; set; }

	    public List<string> Emails { get; set; }
    }
}