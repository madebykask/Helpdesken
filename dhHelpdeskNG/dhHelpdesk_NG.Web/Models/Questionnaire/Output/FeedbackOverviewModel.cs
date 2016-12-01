using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Models.Questionnaire.Output
{
	public class FeedbackOverviewModel: QuestionnaireOverviewModel
	{
		public FeedbackOverviewModel()
		{
		}

		public FeedbackOverviewModel(int id, string name, string description) : base(id, name, description)
		{
		}

		public string Identifier { get; set; }
	}
}