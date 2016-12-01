﻿using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;

namespace DH.Helpdesk.BusinessData.Models.Feedback
{
	public sealed class FeedbackOverview : QuestionnaireOverview
	{
		public FeedbackOverview()
		{
		}

		public FeedbackOverview(int id, string name, string description) : base(id, name, description)
		{
		}

		public string Identifier { get; set; }
	}
}
