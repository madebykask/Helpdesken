using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Web.Models.Feedback
{
	public class FeedbackAnswerParams
	{
		public Guid Guid { get; set; }
		public int LanguageIds { get; set; }
		public int OptionValue { get; set; }
	}
}