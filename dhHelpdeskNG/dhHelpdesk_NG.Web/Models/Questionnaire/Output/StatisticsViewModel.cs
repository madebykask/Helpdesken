namespace DH.Helpdesk.Web.Models.Questionnaire.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Read;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Response.Questionnaire;

    public class StatisticsViewModel
    {
        public StatisticsViewModel(
            int questionnaireId,
            QuestionnaireOverview questionnaireOverview,
            List<OptionResult> optionResults)
        {
            this.QuestionnaireId = questionnaireId;
            this.QuestionnaireOverview = questionnaireOverview;
            this.OptionResults = optionResults;
        }

        [IsId]
        public int QuestionnaireId { get; set; }

        public QuestionnaireOverview QuestionnaireOverview { get; set; }

        public List<OptionResult> OptionResults { get; set; }
    }
}