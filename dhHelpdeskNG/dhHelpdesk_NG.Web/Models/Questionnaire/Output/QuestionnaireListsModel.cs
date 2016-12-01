using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Models.Questionnaire.Output
{
	public class QuestionnaireListsModel
	{
		public QuestionnaireListsModel()
		{
			Questionnaires = new List<QuestionnaireOverviewModel>();
			Feedbacks = new List<FeedbackOverviewModel>();
		}

		public IList<DH.Helpdesk.Web.Models.Questionnaire.Output.QuestionnaireOverviewModel> Questionnaires;
		public IList<DH.Helpdesk.Web.Models.Questionnaire.Output.FeedbackOverviewModel> Feedbacks;
	}
}