using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

namespace DH.Helpdesk.Web.Models.Feedback
{
	public class AddQuestionOptionParams
	{
		public int FeedbackId { get; set; }

		public int QuestionId { get; set; }

		public int LanguageId { get; set; }

		public int OptionPos { get; set; }

		public 	string OptionText { get; set; }

		public int OptionValue { get; set; }

		[LocalizedStringLength(200)]
		public string OptionIcon { get; set; }

        public string IconSrc { get; set; }
	}
}