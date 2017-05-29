using System;

namespace DH.Helpdesk.Web.Models.Feedback
{
	public class FeedbackAnswerParams
	{
		public Guid Guid { get; set; }
		public int LanguageId { get; set; }
		public int OptionId { get; set; }
		public int CustomerId { get; set; }
	}
}