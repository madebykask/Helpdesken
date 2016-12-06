using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Feedback
{
	public class FeedbackQuestionOverview
	{
		public int Id { get; set; }

		public string Question { get; set; }

		public string Number { get; set; }

		public bool IsShowNote { get; set; }

		public string NoteText { get; set; }
	}
}
