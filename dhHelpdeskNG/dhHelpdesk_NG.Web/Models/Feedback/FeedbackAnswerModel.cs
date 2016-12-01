using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

namespace DH.Helpdesk.Web.Models.Feedback
{
	public class FeedbackAnswerModel
	{
		[LocalizedDisplay("LanguageId")]
		public int LanguageId { get; set; }

		public bool IsAnonym { get; set; }

		public Guid Guid { get; set; }

		public int OptionId { get; set; }

		public bool IsShowNote { get; set; }

		public string NoteTextLabel { get; set; }

		public string NoteText { get; set; }

		public int CaseId { get; set; }

	}
}