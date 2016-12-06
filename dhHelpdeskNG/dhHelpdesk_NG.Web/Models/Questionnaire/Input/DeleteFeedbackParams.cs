using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Models.Questionnaire.Input
{
	public class DeleteFeedbackParams
	{
		public int FeedbackId { get; set; }
		public int LanguageId { get; set; }
		public int QuestionId { get; set; }
		public int CircularId { get; set; }
	}
}