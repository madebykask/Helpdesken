using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Models.Feedback
{
	public class DeleteQuestionOptionParams
	{
		public int FeedbackId { get; set; }

		public int QuestionId { get; set; }
	
		public int LanguageId { get; set; }

		public int OptionId { get; set; }
	}
}