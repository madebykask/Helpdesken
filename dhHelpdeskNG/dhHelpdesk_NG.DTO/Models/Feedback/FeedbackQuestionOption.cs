using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Feedback
{
	public class FeedbackQuestionOption
	{
		[IsId]
		public int Id { get; set; }

		[MinValue(0)]
		public int Position { get; set; }

		[NotNullAndEmpty]
		public string Text { get; set; }

		[MinValue(0)]
		public int Value { get; set; }

		[MaxLength(200)]
		public string IconId { get; set; }

	    public string IconSrc { get; set; }

	}
}
