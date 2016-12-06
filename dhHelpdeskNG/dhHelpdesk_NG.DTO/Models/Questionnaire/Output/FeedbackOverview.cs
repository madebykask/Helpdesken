using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Output
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
